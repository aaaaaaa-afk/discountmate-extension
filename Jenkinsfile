pipeline {
    agent any

    options {
        disableConcurrentBuilds()
    }
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
                // use docker image to build app and run in container
                bat 'docker build -t discountmate:test .'

                // remove the previous test container if one exists.
                script {
                    def containerExists = bat(
                        script: 'docker container inspect discountmate-test >nul 2>&1',
                        returnStatus: true
                    )

                    if (containerExists == 0) {
                    bat 'docker rm -f discountmate-test'
                }
            }

            // start application at localhost:5081
            bat 'docker run -d --name discountmate-test -p 127.0.0.1:5081:8080 discountmate:test'

            // check that the application is running and responding to requests
            // retry up to 10 times, with a 2 second delay between attempts, and a maximum time of 5 seconds for each request
            // not respoinding  = fail
            bat 'curl.exe --fail --silent --show-error --retry 10 --retry-connrefused --retry-delay 2 --max-time 5 --output NUL http://localhost:5081/'
            }
        }

        stage('Release') {
            steps {
                // release label with bbuild number
                bat 'docker tag discountmate:test discountmate:release-%BUILD_NUMBER%'

                // check if previous container exists remove if do
                script {
                    def containerExists = bat(
                        script: 'docker container inspect discountmate-production >nul 2>&1',
                        returnStatus: true
                    )

                // remove previous container if it exists. New container start with same name, so the old one must be removed first
                if (containerExists == 0) {
                bat 'docker rm -f discountmate-production'
                }
            }

                // start container with localhost:5082 mapping to 8080 set as production environment
                bat 'docker run -d --name discountmate-production -e ASPNETCORE_ENVIRONMENT=Production -p 127.0.0.1:5082:8080 discountmate:release-%BUILD_NUMBER%'

                // check app is running and responding 
                bat 'curl.exe --fail --silent --show-error --retry 10 --retry-connrefused --retry-delay 2 --max-time 5 --output NUL http://localhost:5082/'
            }
        }

        stage('Monitoring') {
            steps {
                // check existing kuma container is running
                bat 'docker start discountmate-monitor'
                // dashboard check responses
                bat 'curl.exe --fail --silent --show-error --retry 10 --retry-connrefused --retry-delay 2 --max-time 5 --output NUL http://localhost:3001/'

                echo 'Uptime Kuma: http://localhost:3001'
            }
        }
    }
}