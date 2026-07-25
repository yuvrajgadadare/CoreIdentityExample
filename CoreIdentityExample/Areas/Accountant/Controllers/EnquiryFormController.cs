using ERP_Models;
using ERP_Services.Implementations;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace CoreIdentityExample.Areas.Accountant.Controllers
{
    [Area("Accountant")]
    [Authorize]
    public class EnquiryFormController : Controller
    {
        IMasterService masterService;
        IBranchService branchService;
        IEnquiryService enquiryService;
        IExtraService extraService;
        EmailSettings _settings;
        ITopicService topicService;
        IEmployeeService employeeService;
        //public ExtraService(IOptions<EmailSettings> settings)
        //{
        //    _settings = settings.Value;
        //}
        public EnquiryFormController(IOptions<EmailSettings> settings, IMasterService masterService, IEnquiryService enquiryService, IExtraService extraService, IBranchService branchService, ITopicService topicService, IEmployeeService employeeService)
        {
            this.masterService = masterService;
            this.enquiryService = enquiryService;
            this.extraService = extraService;
            _settings = settings.Value;
            this.branchService = branchService;
            this.topicService = topicService;
            this.employeeService = employeeService;
        }
        public async Task<IActionResult> Index()
        {

            ViewData["branches"] = await branchService.GetBranches();
            ViewData["qualifications"] = await masterService.GetQualifications();
            EnquiryModel em = new EnquiryModel()
            {
                enquiry_forList = await masterService.GetEnquiryFors(),
                lead_sourceList = await masterService.GetLeadSources(),
                topicList = await topicService.GetTrainingTopics(),
            };

            return View(em);
        }
        [HttpPost]
        public async Task<IActionResult> Index(EnquiryModel enquiry)
        {

            string enquiryfordata = "";
            foreach (EnquiryForModel e in enquiry.enquiry_forList)
            {
                if (e.is_selected)
                {
                    enquiryfordata += "," + e.enquiry_for;
                }

            }
            enquiryfordata = enquiryfordata.Substring(1, enquiryfordata.Length - 1);
            string leadsourcedata = "";
            foreach (LeadSourceModel e in enquiry.lead_sourceList)
            {
                if (e.is_selected)
                {
                    leadsourcedata += "," + e.source_name;
                }
            }
            leadsourcedata = leadsourcedata.Substring(1, leadsourcedata.Length - 1);



            string topicdata = "";
            foreach (TopicModel e in enquiry.topicList)
            {
                if (e.is_selected)
                {
                    topicdata += "," + e.topic_name;
                }
            }
            topicdata = topicdata.Substring(1, topicdata.Length - 1);
            enquiry.enquiry_fors = enquiryfordata;
            enquiry.lead_sources = leadsourcedata;
            enquiry.interested_topics = topicdata;
            enquiry.status = "Enquiry Submitted";
            await enquiryService.AddEnquiry(enquiry);
            EmailModel emodel = new EmailModel()
            {
                EmailAddress = enquiry.email_address,
                Message = "<h2>Dear " + enquiry.candidate_name + ",</h2><p>Thank you for inquiring about <b>" + enquiry.interested_topics + "</b>.</p><p> We will be in touch in less than an hour to answer any questions you have.</p><p>Please feel free to check out courses  here <a href='https://ciitinstitute.com/' target='_blank'>CIIT Training Institute</a></p><br/><br/> <h2>Regards,</h2><h2>CIIT Training Institute Pvt Ltd</h2>",
                Subject = "Enquiry With CIIT Training Institute",
                UserName = enquiry.candidate_name
            };
            await extraService.SendEmail(emodel, _settings);
            ModelState.Clear();
            ViewBag.msg = "Your enquiry has been submitted successfully.";
            ViewData["branches"] = await branchService.GetBranches();
            ViewData["qualifications"] = await masterService.GetQualifications();
            EnquiryModel em = new EnquiryModel()
            {
                enquiry_forList = await masterService.GetEnquiryFors(),
                lead_sourceList = await masterService.GetLeadSources(),
                topicList = await topicService.GetTrainingTopics(),
            };

            return View(em);
        }

        public async Task<IActionResult> AllEnquiries()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            EmployeeModel d = await employeeService.GetEmployeeByUserId(userId);
            List<EnquiryModel> lst = await enquiryService.GetEnquiries(d.branch_id);
            return View(lst);
        }
    }
}
