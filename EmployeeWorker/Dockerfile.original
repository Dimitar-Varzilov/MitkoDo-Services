FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EmployeeWorker.csproj", "EmployeeWorker/"]
RUN dotnet restore "EmployeeWorker/EmployeeWorker.csproj"

COPY . EmployeeWorker/.
RUN dotnet publish -c Release -o /app EmployeeWorker/EmployeeWorker.csproj 

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS publish
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "EmployeeWorker.dll"]