FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY BuildOutput/ .

# port 8080
ENV ASPNETCORE_HTTP_PORTS=8080

USER app

# sttart app
ENTRYPOINT ["dotnet", "DiscountMate.dll"]