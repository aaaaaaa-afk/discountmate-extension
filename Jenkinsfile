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

                    withEnv(["SCANNER_HOME=${scannerHome}"]) {
                        withSonarQubeEnv('LocalSonar') {
                            bat '@dotnet "%SCANNER_HOME%\\SonarScanner.MSBuild.dll" begin /k:"discountmate" /d:sonar.host.url="%SONAR_HOST_URL%" /d:sonar.token="%SONAR_AUTH_TOKEN%" /d:sonar.exclusions="BuildOutput/**"'
                            bat 'dotnet build App/DiscountMate/DiscountMate.csproj -c Release --no-incremental'
                            bat '@dotnet "%SCANNER_HOME%\\SonarScanner.MSBuild.dll" end /d:sonar.token="%SONAR_AUTH_TOKEN%"'
                        }
                    }
                }
            }
        }

        stage('Security') {
            steps {
                //NuGet Audit is a tool that checks for known vulnerabilities in NuGet packages. The command below restores the test project and checks for vulnerabilities in all packages, treating any warnings as errors.
                bat 'dotnet restore Tests/DiscountMate.Tests.csproj --force -p:NuGetAudit=true -p:NuGetAuditMode=all -p:NuGetAuditLevel=low -warnaserror'
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