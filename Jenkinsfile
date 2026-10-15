pipeline {
    agent any

    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        DOTNET_NOLOGO = '1'
    }

    stages {
        stage('Info') {
            steps {
                sh 'dotnet --info'
            }
        }

    stage('Restore') {
        steps {
            sh 'dotnet restore ArtistHelper.csproj -r win-x64 -p:EnableWindowsTargeting=true'
        }
    }

    stage('Build') {
        steps {
            sh 'dotnet build ArtistHelper.csproj -c Release -r win-x64 --no-restore -p:EnableWindowsTargeting=true'
        }
    }

    stage('Publish') {
        steps {
            sh 'dotnet publish ArtistHelper.csproj -c Release -r win-x64 --self-contained false --no-restore -p:EnableWindowsTargeting=true -o ./publish'
        }
    }
    }

    post {
        success {
            archiveArtifacts artifacts: 'publish/**/*', fingerprint: true
        }
        cleanup {
            cleanWs()
        }
    }
}