pipeline {
    agent none

    stages {
        stage('Build') {
			agent any
            steps {
                
                sh "docker build . -f ./AcoesGRPC/Dockerfile -t luizcarlosfaria/grpc-on-tdc-floripa:${env.BUILD_ID}"
                
            }
        }

        stage('Push') {
            agent any
            when {
              expression {
                currentBuild.result == null || currentBuild.result == 'SUCCESS' 
              }
            }
            steps {
                echo "publishing  luizcarlosfaria/grpc-on-tdc-floripa:${env.BUILD_ID}"
                
                sh "docker tag luizcarlosfaria/grpc-on-tdc-floripa:${env.BUILD_ID} luizcarlosfaria/grpc-on-tdc-floripa:latest"

                sh "docker push luizcarlosfaria/grpc-on-tdc-floripa:${env.BUILD_ID}"
                
                sh "docker push luizcarlosfaria/grpc-on-tdc-floripa:latest"
               
                echo "published luizcarlosfaria/grpc-on-tdc-floripa:${env.BUILD_ID} | on ${env.JENKINS_URL}"
            }
        }
    }
}