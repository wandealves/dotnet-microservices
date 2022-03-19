1 - Criar MongoDB

docker container run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo

2 - Adicionar Pacotes ao Projey

dotnet nuget add source (Caminho do Pacote) -n PlayEconomy
