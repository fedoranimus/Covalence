FROM microsoft/dotnet:2.1-runtime
WORKDIR /Covalence
ENV ASPNETCORE_URLS http://+:80
EXPOSE 80
COPY Covalence/bin/Release/netcoreapp2.1/publish/ .
ENTRYPOINT ["dotnet", "Covalence.dll"]
