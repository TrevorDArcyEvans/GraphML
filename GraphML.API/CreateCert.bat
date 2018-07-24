openssl req -new -x509 -newkey rsa:2048 -keyout GraphML.key -out GraphML.cer -days 3650 -subj /CN=GraphML
openssl pkcs12 -export -out GraphML.pfx -inkey GraphML.key -in GraphML.cer
