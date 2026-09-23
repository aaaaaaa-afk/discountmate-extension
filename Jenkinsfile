pipeline {
    agent any

    stages {
        stage('Build') {
            steps {
                bat 'dotnet publish App/DiscountMate/DiscountMate.csproj -c Release -o BuildOutput'
                archiveArtifacts 'BuildOutput/**'
            }
        }

        stage('Test') {
            steps {
                bat 'dotnet test Tests/DiscountMate.Tests.csproj -c Release'
            }
        }

        stage('Code Quality') {
            steps {
                echo 'Code quality analysis is not configured yet.'
            }
        }

        stage('Security') {
            steps {
                echo 'Security scanning is not configured yet.'
            }
        }

        stage('Deployment') {
            steps {
                echo 'Test deployment is not configured yet.'
            }
        }

        stage('Release') {
            steps {
                echo 'Production release is not configured yet.'
            }
        }

        stage('Monitoring') {
            steps {
                echo 'Monitoring and alerting are not configured yet.'
            }
        }
    }
}