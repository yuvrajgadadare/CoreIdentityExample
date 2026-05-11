pipeline {
    agent any
    environment {
        DOTNET_CLI_HOME="C:\\Program Files\\dotnet"
    }
    stages {
        stage("Checkout") {
         steps {
               checkout scm
         }
        }
        stage("Build") {
            steps {
                bat "dotnet restore"
                bat "dotnet build --configuration Release"
            }
        }
        stage("Test") {
            steps {
                bat "dotnet test --no-restore --configuration Release"
            }

        }
        stage("Publish") {
            steps {
                script {
                    bat "dotnet publish --no-restore --configuration Release --output .\\publish"
                }
            }
        }

    }
    post {
        success {
            echo "Build, TEst and Publish stages completed successfully."
        }
    }

}