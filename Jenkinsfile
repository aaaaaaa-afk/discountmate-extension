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
                script {
                     def scannerHome = tool 'SonarScanner for .NET'

                    withSonarQubeEnv('LocalSonar') {
                        bat "dotnet \"${scannerHome}\\SonarScanner.MSBuild.dll\" begin /k:\"discountmate\" /d:sonar.exclusions=\"BuildOutput/**\""
                        bat 'dotnet build App/DiscountMate/DiscountMate.csproj -c Release --no-incremental'
                        bat "dotnet \"${scannerHome}\\SonarScanner.MSBuild.dll\" end"
                    }
                }
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