pipeline {
    agent none

    stages {
        stage('Build') {
			agent any
            steps {
                
                sh "docker build . -f ./AcoesGRPC/Dockerfile -t luizcarlosfaria/grpc-on-tdc-floripa:${env.BUILD_ID}"
                
            }
        }

        stage('Deploy') {
            when {
              expression {
                currentBuild.result == null || currentBuild.result == 'SUCCESS' 
              }
            }
            steps {
                echo "publishing  luizcarlosfaria/grpc-on-tdc-floripa:${env.BUILD_ID}"
                
                sh "docker push luizcarlosfaria/grpc-on-tdc-floripa:${env.BUILD_ID}"
                
                echo "published luizcarlosfaria/grpc-on-tdc-floripa:${env.BUILD_ID} | on ${env.JENKINS_URL}"
            }
        }
    }
}