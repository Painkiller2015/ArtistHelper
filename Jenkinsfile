pipeline {
    agent any
    
    // Настройки для предотвращения лишнего шума в логах
    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        DOTNET_NOLOGO = '1'
    }

    stages {
        stage('Checkout') {
            steps {
                // Этот шаг уже выполняется Jenkins автоматически, но полезен для наглядности
                checkout scm
                sh 'dotnet --info' // Проверка, что .NET установлен на агенте
            }
        }
        
        stage('Restore') {
            steps {
                // Восстанавливаем NuGet-пакеты для всего решения
                sh 'dotnet restore ArtistHelper.sln'
            }
        }
        
        stage('Build') {
            steps {
                // Собираем проект в конфигурации Release
                sh 'dotnet build ArtistHelper.sln -c Release --no-restore'
            }
        }
        
        stage('Publish') {
            steps {
                // Публикуем приложение. Это создаст артефакты для распространения.
                // Для WPF-приложения важно указать runtime, например win-x64.
                // Параметры self-contained и PublishSingleFile можно настроить по желанию.
                sh 'dotnet publish ArtistHelper.csproj -c Release -r win-x64 --self-contained false -o ./publish'
            }
        }
    }
    
    post {
        always {
            // Архивируем результаты публикации, чтобы их можно было скачать из Jenkins
            archiveArtifacts artifacts: 'publish/**/*', fingerprint: true, allowEmptyArchive: true
            // Очищаем рабочее пространство после сборки
            cleanWs()
        }
    }
}