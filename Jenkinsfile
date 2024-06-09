pipeline {
    agent any

    environment {
        // Define environment variables
        NODE_HOME = '/usr/local/bin'
        PATH = "$NODE_HOME:$PATH"
    }

    stages {

        stage('Initialize'){
          def dockerHome = tool 'myDocker'
          env.PATH = "${dockerHome}/bin:${env.PATH}"
        }

        stage('Build frontend') {
            agent {
              docker {
                image 'node:22-alpine3.19'
                args '-v /var/run/docker.sock:/var/run/docker.sock'
              }
            }
            steps {
                sh 'npm install'
                sh 'npm run build'

                script {
                    docker.build('giga-inhouse-frontend:latest', './giga-inhouse-frontend/Dockerfile')
                }

                script {
                    // Run the Docker container
                    docker.withRegistry('', 'docker-credentials-id') {
                        // Stop and remove existing container if it exists
                        def containerExists = sh(script: "docker ps -q -f name=giga-inhouse-frontend", returnStdout: true).trim()
                        if (containerExists) {
                            sh "docker stop giga-inhouse-frontend"
                            sh "docker rm giga-inhouse-frontend"
                        }
                        // Run a new container with the built image
                        sh 'docker run -d --name giga-inhouse-frontend -p 5001:80 giga-inhouse-frontend:latest'
                    }
                }
            }
        }
    }

    post {
        always {
            // Clean up workspace
            cleanWs()
        }
        success {
            echo 'Deployment successful!'
        }
        failure {
            echo 'Deployment failed.'
        }
    }
}
