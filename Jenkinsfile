pipeline {
    agent any

    environment {
        // Define environment variables
        NODE_HOME = '/usr/local/bin'
        PATH = "$NODE_HOME:$PATH"
    }

    stages {
        stage('Install Dependencies') {
            steps {
                sh 'npm install'
            }
        }

        stage('Build Project') {
            steps {
                sh 'npm run build'
            }
        }

        stage('Build Docker Image') {
            steps {
                script {
                    docker.build('giga-inhouse-frontend:latest', './giga-inhouse-frontend/Dockerfile')
                }
            }
        }

        stage('Deploy to Docker') {
            steps {
                script {
                    // Run the Docker container
                    docker.withRegistry('', 'docker-credentials-id') {
                        // Stop and remove existing container if it exists
                        sh """
                        if [ $(docker ps -q -f name=giga-inhouse-frontend) ]; then
                            docker stop giga-inhouse-frontend
                            docker rm giga-inhouse-frontend
                        fi
                        """
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
