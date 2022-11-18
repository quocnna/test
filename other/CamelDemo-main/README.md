# ActiveMQ docker can be up and running using the following command
## docker run -p 61616:61616 -p 8161:8161 rmohr/activemq

https://guttikondaparthasai.medium.com/apache-camel-spring-boot-5cc1cdd21cea

- activemq from docker, query vs topoc
- procedure: missing @component
- consumer: version down, Which version of Camel are you using? If it is later than Camel 3, you need to import camel-direct in your pom file as the direct component has been moved out of the camel-core module.
