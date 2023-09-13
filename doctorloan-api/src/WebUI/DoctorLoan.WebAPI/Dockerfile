#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/WebUI/DoctorLoan.WebAPI/DoctorLoan.WebAPI.csproj", "src/WebUI/DoctorLoan.WebAPI/"]
COPY ["src/Core/DoctorLoan.Application/DoctorLoan.Application.csproj", "src/Core/DoctorLoan.Application/"]
COPY ["src/Core/DoctorLoan.Domain/DoctorLoan.Domain.csproj", "src/Core/DoctorLoan.Domain/"]
COPY ["src/Core/DoctorLoan.Infrastructure/DoctorLoan.Infrastructure.csproj", "src/Core/DoctorLoan.Infrastructure/"]
COPY ["src/Modules/Authenticates/DoctorLoan.Authen.WebUI/DoctorLoan.Authen.WebUI.csproj", "src/Modules/Authenticates/DoctorLoan.Authen.WebUI/"]
COPY ["src/Modules/Authenticates/DoctorLoan.Authen.Application/DoctorLoan.Authen.Application.csproj", "src/Modules/Authenticates/DoctorLoan.Authen.Application/"]
COPY ["src/Modules/Authenticates/DoctorLoan.Authen.Infrastructure/DoctorLoan.Authen.Infrastructure.csproj", "src/Modules/Authenticates/DoctorLoan.Authen.Infrastructure/"]
COPY ["src/Modules/Bookings/DoctorLoan.Booking.WebUI/DoctorLoan.Booking.WebUI.csproj", "src/Modules/Bookings/DoctorLoan.Booking.WebUI/"]
COPY ["src/Modules/Bookings/DoctorLoan.Booking.Application/DoctorLoan.Booking.Application.csproj", "src/Modules/Bookings/DoctorLoan.Booking.Application/"]
COPY ["src/Modules/Customers/DoctorLoan.Customer.Application/DoctorLoan.Customer.Application.csproj", "src/Modules/Customers/DoctorLoan.Customer.Application/"]
COPY ["src/Modules/Commons/DoctorLoan.Common.WebUI/DoctorLoan.Common.WebUI.csproj", "src/Modules/Commons/DoctorLoan.Common.WebUI/"]
COPY ["src/Modules/Commons/DoctorLoan.Common.Application/DoctorLoan.Common.Application.csproj", "src/Modules/Commons/DoctorLoan.Common.Application/"]
COPY ["src/Modules/Commons/DoctorLoan.Common.Infrastructure/DoctorLoan.Common.Infrastructure.csproj", "src/Modules/Commons/DoctorLoan.Common.Infrastructure/"]
COPY ["src/Modules/Customers/DoctorLoan.Customer.WebUI/DoctorLoan.Customer.WebUI.csproj", "src/Modules/Customers/DoctorLoan.Customer.WebUI/"]
COPY ["src/Modules/Documents/DoctorLoan.Documents.WebUI/DoctorLoan.Contents.WebUI.csproj", "src/Modules/Documents/DoctorLoan.Documents.WebUI/"]
COPY ["src/Modules/Documents/DoctorLoan.Documents.Application/DoctorLoan.Contents.Application.csproj", "src/Modules/Documents/DoctorLoan.Documents.Application/"]
COPY ["src/Modules/Documents/DoctorLoan.Documents.Domain/DoctorLoan.Contents.Infrastructure.csproj", "src/Modules/Documents/DoctorLoan.Documents.Domain/"]
COPY ["src/Modules/Products/DoctorLoan.Products.WebUI/DoctorLoan.Products.WebUI.csproj", "src/Modules/Products/DoctorLoan.Products.WebUI/"]
COPY ["src/Modules/Products/DoctorLoan.Products.Application/DoctorLoan.Products.Application.csproj", "src/Modules/Products/DoctorLoan.Products.Application/"]
COPY ["src/Modules/Users/DoctorLoan.User.WebUI/DoctorLoan.User.WebUI.csproj", "src/Modules/Users/DoctorLoan.User.WebUI/"]
COPY ["src/Modules/Users/DoctorLoan.User.Application/DoctorLoan.User.Application.csproj", "src/Modules/Users/DoctorLoan.User.Application/"]
COPY ["src/Modules/Users/DoctorLoan.User.Infrastructure/DoctorLoan.User.Infrastructure.csproj", "src/Modules/Users/DoctorLoan.User.Infrastructure/"]
RUN dotnet restore "src/WebUI/DoctorLoan.WebAPI/DoctorLoan.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/WebUI/DoctorLoan.WebAPI"
RUN dotnet build "DoctorLoan.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DoctorLoan.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DoctorLoan.WebAPI.dll"]