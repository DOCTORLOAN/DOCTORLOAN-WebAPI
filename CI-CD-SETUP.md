# Hướng dẫn thiết lập CI/CD cho DOCTORLOAN-WebAPI

Tài liệu này hướng dẫn cách thiết lập và cấu hình CI/CD (Continuous Integration/Continuous Deployment) pipeline cho project DOCTORLOAN-WebAPI với chuẩn hóa code, clean code, security scanning, performance analysis, **API validation**, **API testing** và tự động tạo GitHub Issues.

## 📋 Tổng quan

Project sử dụng **GitHub Actions** để tự động kiểm tra code quality và **API validation** trên **TẤT CẢ CÁC BRANCH**:
- ✅ Build và test code
- ✅ Kiểm tra code formatting (EditorConfig)
- ✅ Kiểm tra code style (StyleCop)
- ✅ Code analysis (Roslyn Analyzers)
- ✅ **Security scanning** (NuGet vulnerabilities, hardcoded secrets, security code analysis)
- ✅ **Performance analysis** (code complexity, anti-patterns, optimization suggestions)
- ✅ **API Validation & Documentation** (OpenAPI/Swagger validation, API contract consistency)
- ✅ **API Endpoint Testing** (Health checks, Swagger endpoints, OpenAPI spec)
- ✅ **API Configuration Validation** (JWT, CORS, Rate Limiting, Security Headers)
- ✅ **Tự động tạo GitHub Issues** với severity labels
- ✅ Code smells detection
- ✅ Comprehensive reporting

## 🚀 Workflow Structure

### `.github/workflows/ci.yml` - CI Pipeline

Workflow được chia thành các **parallel jobs** để tối ưu thời gian chạy:

1. **Code Quality & Formatting** (Job 1)
   - Code formatting verification
   - Build với warnings tracking
   - Code analysis với .NET Analyzers
   - Code smells detection

2. **Security Analysis** (Job 2) - Chạy song song với Job 1
   - NuGet vulnerability scanning
   - Hardcoded secrets detection (bao gồm JWT keys)
   - Security code analysis (OWASP, CWE checks)

3. **Performance Analysis** (Job 3) - Chạy song song
   - Code complexity analysis
   - Performance anti-patterns detection
   - Optimization suggestions

4. **API Validation & Documentation** (Job 4) - **API-specific**
   - OpenAPI/Swagger specification generation
   - OpenAPI specification validation
   - API documentation quality checks
   - API configuration validation (JWT, CORS, Rate Limiting)
   - API security headers validation
   - API endpoints discovery
   - API contract consistency validation
   - Health check endpoint validation

5. **API Endpoint Testing** (Job 5) - **API-specific**
   - Health check endpoint testing
   - Swagger endpoint testing
   - OpenAPI specification endpoint testing
   - API server startup validation

6. **API Testing** (Job 6) - Placeholder cho future test projects
   - Unit tests (khi có test projects)
   - Integration tests (khi có test projects)

7. **Create GitHub Issues** (Job 7) - Chạy sau khi các jobs khác hoàn thành
   - Tự động tạo issues từ security findings
   - Tự động tạo issues từ performance findings
   - Tự động tạo issues từ API validation findings
   - Phân loại severity: Critical, High, Medium, Low, Enhancement

8. **Generate Quality Report** (Job 8)
   - Tổng hợp báo cáo từ tất cả các jobs
   - Hiển thị summary trong GitHub Actions
   - API-specific metrics và statistics

### Trigger Events

- **Push** vào bất kỳ branch nào (`**`)
- **Pull Request** vào bất kỳ branch nào
- **Manual trigger** (workflow_dispatch)

## ⚙️ Cấu hình

### Files cấu hình

1. **`.editorconfig`** - Chuẩn hóa code formatting
   - C# coding conventions
   - Naming conventions
   - Formatting rules
   - Indentation preferences

2. **`stylecop.json`** - StyleCop configuration (nếu có)
   - Documentation rules
   - Naming rules
   - Layout rules
   - Ordering rules

3. **`Directory.Build.props`** - Shared build properties (nếu có)
   - Enable analyzers
   - Analysis level
   - Warning configuration

4. **`doctorloan-api/src/WebUI/DoctorLoan.WebAPI/DoctorLoan.WebAPI.csproj`** - Project configuration
   - NuGet packages cho analyzers
   - Code analysis settings
   - NSwag configuration

5. **`doctorloan-api/src/WebUI/DoctorLoan.WebAPI/nswag.json`** - NSwag configuration
   - OpenAPI specification generation
   - Swagger UI configuration
   - Client generation settings

6. **`doctorloan-api/src/WebUI/DoctorLoan.WebAPI/appsettings.json`** - API Configuration
   - JWT Token Configuration
   - Rate Limiting Configuration
   - CORS Configuration
   - Health Check Configuration
   - Database Connection Strings

### Scripts hỗ trợ

Scripts có thể được thêm vào `.github/scripts/`:
- `create-github-issue.py` - Tạo GitHub Issues
- `parse-security-issues.js` - Parse security findings
- `parse-performance-issues.js` - Parse performance findings
- `analyze-complexity.py` - Analyze code complexity
- `export-secret-findings.py` - Export secret findings

### NuGet Packages

Project sử dụng các packages sau cho code analysis và API:
- `Microsoft.CodeAnalysis.NetAnalyzers` - .NET Code Analyzers
- `NSwag.AspNetCore` - Swagger/OpenAPI generation
- `NSwag.Generation.AspNetCore` - OpenAPI specification generation
- `Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore` - Health checks
- `System.Threading.RateLimiting` - Rate limiting

### GitHub Permissions

Workflow cần các permissions sau:
- `contents: read` - Đọc code
- `issues: write` - Tạo issues
- `security-events: write` - Security scanning (optional)

**Lưu ý**: `GITHUB_TOKEN` được tự động cung cấp bởi GitHub Actions, không cần cấu hình thêm.

## 🔒 Security Scanning

### 1. NuGet Vulnerability Scanning

Pipeline tự động kiểm tra các package có lỗ hổng bảo mật:

```bash
dotnet list package --vulnerable --include-transitive
```

**Severity Classification:**
- **High**: Vulnerable packages với CVSS score cao
- **Medium**: Vulnerable packages với CVSS score trung bình

**Gợi ý**: Update packages lên phiên bản mới nhất không có lỗ hổng.

### 2. Hardcoded Secrets Detection

Pipeline quét code để tìm hardcoded secrets:

- Passwords
- API keys
- Tokens
- Connection strings
- **JWT keys** (đặc biệt quan trọng cho Web API)

**Severity Classification:**
- **Critical**: Hardcoded secrets trong production code

**Gợi ý**: 
- Sử dụng configuration files (appsettings.json, environment variables)
- Sử dụng Azure Key Vault hoặc similar services
- Không commit secrets vào repository
- Sử dụng `dotnet user-secrets` cho development

### 3. Security Code Analysis

Pipeline chạy security analyzers để phát hiện:

- SQL Injection vulnerabilities
- XSS vulnerabilities
- Insecure deserialization
- Weak cryptography
- Authentication/Authorization issues
- Input validation issues
- **API security misconfigurations**

**Severity Classification:**
- **Critical**: SQL injection, XSS, authentication bypass
- **High**: Weak crypto, insecure deserialization
- **Medium**: Missing validation, insecure configuration

## ⚡ Performance Analysis

### 1. Code Complexity Analysis

Pipeline phân tích:
- Files quá dài (>500 lines)
- Methods quá phức tạp
- Cyclomatic complexity

**Severity Classification:**
- **Enhancement**: Large files, high complexity

**Gợi ý**: 
- Chia nhỏ files thành các classes nhỏ hơn
- Extract methods để giảm complexity
- Apply SOLID principles

### 2. Performance Anti-patterns Detection

Pipeline phát hiện:
- **Blocking async operations** (`.Result`, `.Wait()`, `.GetAwaiter().GetResult()`)
- **String concatenation trong loops**
- **Missing async/await** trên I/O operations
- **Inefficient LINQ queries**
- **Large memory allocations**
- **Synchronous database calls**

**Severity Classification:**
- **Critical**: Blocking async operations, memory leaks
- **High**: Missing async, large allocations
- **Medium**: String concatenation, LINQ inefficiencies
- **Enhancement**: Minor optimizations

**Gợi ý**:
- Sử dụng `async/await` đúng cách
- Sử dụng `StringBuilder` cho string concatenation trong loops
- Sử dụng async I/O operations
- Optimize LINQ queries
- Sử dụng async database operations

## 🔌 API Validation & Documentation

### 1. OpenAPI/Swagger Specification Validation

Pipeline tự động:
- Generate OpenAPI specification từ code
- Validate OpenAPI specification format
- Kiểm tra OpenAPI specification completeness

**Validation Checks:**
- ✅ OpenAPI specification is valid JSON
- ✅ All endpoints have response schemas
- ✅ All endpoints have request body schemas (nếu cần)
- ✅ API version is specified
- ✅ Server URLs are defined

**Nếu fail:**
- Kiểm tra NSwag configuration trong `nswag.json`
- Đảm bảo controllers có đầy đủ XML documentation
- Kiểm tra `[ApiController]` attributes

### 2. API Documentation Quality

Pipeline kiểm tra:
- Endpoints có documentation (summary, description)
- Response schemas đầy đủ
- Request body schemas đầy đủ
- Authentication requirements được document
- Error responses được document

**Severity Classification:**
- **Medium**: Missing documentation
- **Low**: Incomplete documentation

**Gợi ý**:
- Thêm XML comments cho controllers và actions
- Document request/response models
- Document error responses
- Document authentication requirements

### 3. API Configuration Validation

Pipeline kiểm tra:
- **JWT Configuration**: Key, Issuer, Audience
- **Rate Limiting Configuration**: Enabled, PermitLimit, WindowSeconds
- **CORS Configuration**: Allowed origins
- **Health Check Configuration**: Endpoint path, checks registered

**Severity Classification:**
- **Critical**: Missing JWT configuration
- **High**: Missing Rate Limiting configuration
- **Medium**: Missing CORS configuration
- **Low**: Missing Health Check configuration

**Gợi ý**:
- Đảm bảo tất cả configuration được set trong `appsettings.json`
- Sử dụng environment-specific configuration files
- Không hardcode configuration values

### 4. API Security Headers Validation

Pipeline kiểm tra:
- Security headers middleware được register
- HTTPS enforcement (UseHttpsRedirection, UseHsts)
- Standard security headers (X-Content-Type-Options, X-Frame-Options, etc.)

**Severity Classification:**
- **High**: Missing security headers
- **Medium**: Missing HTTPS enforcement

**Gợi ý**:
- Implement `UseStandardSecurityHeaders` middleware
- Enable HTTPS redirection
- Enable HSTS cho production

### 5. API Endpoints Discovery

Pipeline tự động:
- Discover tất cả controllers
- Count API routes
- Check `[ApiController]` attributes
- Check authorization attributes (`[Authorize]`, `[AllowAnonymous]`)

**Metrics:**
- Total controllers
- Total API routes
- Endpoints with authentication
- Public endpoints (AllowAnonymous)

### 6. API Contract Consistency

Pipeline kiểm tra:
- Số lượng endpoints trong code khớp với OpenAPI spec
- HTTP methods được sử dụng đúng
- API versioning consistency
- Server URLs consistency

**Severity Classification:**
- **High**: Contract mismatch
- **Medium**: Version inconsistency

### 7. Health Check Endpoint Validation

Pipeline kiểm tra:
- Health checks được register
- Health check endpoint path (`/health`)
- Database health check được configure
- Health check response format

**Severity Classification:**
- **Medium**: Missing health checks
- **Low**: Incomplete health checks

**Gợi ý**:
- Register health checks trong `AddHealthChecks()`
- Configure database health check
- Map health check endpoint trong `UseHealthChecks()`

## 🧪 API Endpoint Testing

### 1. Health Check Endpoint Testing

Pipeline tự động:
- Start API server với test database
- Test `/health` endpoint
- Validate health check response

**Test Scenarios:**
- Health check endpoint returns 200 OK
- Health check response is valid JSON
- Database health check status is reported

### 2. Swagger Endpoint Testing

Pipeline tự động:
- Test Swagger UI endpoint (`/api`)
- Validate Swagger UI is accessible
- Check Swagger UI configuration

### 3. OpenAPI Specification Endpoint Testing

Pipeline tự động:
- Test OpenAPI specification endpoint (`/api/specification.json`)
- Validate OpenAPI spec is valid JSON
- Count endpoints in specification

### 4. API Server Startup Validation

Pipeline tự động:
- Start API server với test configuration
- Validate server starts successfully
- Check server logs for errors

**Test Configuration:**
- PostgreSQL service container
- Test connection string
- Test JWT configuration
- Rate limiting disabled for testing

## 📊 GitHub Issues Auto-Creation

### Severity Labels

Pipeline tự động tạo issues với các labels sau:

- **severity:critical** (🔴 Red) - Issues nghiêm trọng cần fix ngay
- **severity:high** (🟠 Orange) - Issues quan trọng nên fix sớm
- **severity:medium** (🟡 Yellow) - Issues nên được xem xét
- **severity:low** (🟢 Green) - Issues ít ưu tiên
- **severity:enhancement** (🔵 Blue) - Gợi ý cải thiện

### Type Labels

- **type:security** - Security issues
- **type:performance** - Performance issues
- **type:code_quality** - Code quality issues
- **type:api** - API-specific issues

### Issue Format

Mỗi issue tự động được tạo với:

- **Title**: Mô tả ngắn gọn vấn đề
- **Body**: 
  - Description chi tiết
  - Severity và Type
  - File và line number (nếu có)
  - Recommendation
  - Link đến workflow run
- **Labels**: Severity và Type labels

### Duplicate Prevention

Pipeline tự động kiểm tra và tránh tạo issues trùng lặp dựa trên:
- Title matching
- Same severity
- Same file location

## 📝 Sử dụng CI

### Tự động kiểm tra

1. **Push code lên bất kỳ branch nào**:
   ```bash
   git checkout your-branch
   git add .
   git commit -m "Your commit message"
   git push origin your-branch
   ```

2. **GitHub Actions tự động chạy**:
   - Tất cả các jobs chạy song song (code-quality, security-scan, performance-analysis, api-validation)
   - API endpoint testing chạy sau khi API validation pass
   - Tự động tạo issues cho security, performance và API findings
   - Generate comprehensive report

3. **Xem kết quả**:
   - Vào tab **Actions** trong GitHub repository
   - Click vào workflow run để xem chi tiết
   - Xem **Summary** tab để xem comprehensive report
   - Xem **Issues** tab để xem các issues được tạo tự động
   - ✅ Xanh = Pass (code clean, API valid)
   - ❌ Đỏ = Fail (cần sửa lỗi)

### Manual trigger

1. Vào **Actions** tab trong GitHub repository
2. Chọn workflow "CI Pipeline - Code Quality, Security, Performance & API Validation"
3. Click **Run workflow** → Chọn branch → **Run workflow**

## ✅ Code Quality Checks

### 1. Code Formatting

Pipeline sử dụng `dotnet format` để kiểm tra code formatting theo `.editorconfig`:

```bash
dotnet format --verify-no-changes
```

**Nếu fail:**
- Chạy `dotnet format doctorloan-api/DoctorLoan.sln` để tự động fix
- Commit và push lại

### 2. Build Warnings

Pipeline sẽ track và báo cáo warnings trong quá trình build:

```bash
dotnet build --configuration Release
```

**Nếu có warnings:**
- Xem chi tiết trong build logs
- Fix warnings trước khi merge
- Warnings được đếm và hiển thị trong report

### 3. Code Analysis

Pipeline chạy .NET Analyzers để phát hiện:
- Code style violations
- Potential bugs
- Performance issues
- Security vulnerabilities
- Best practice violations

## 🛠️ Fix Code Issues Locally

### Format code tự động

```bash
# Format toàn bộ solution
dotnet format doctorloan-api/DoctorLoan.sln

# Format project cụ thể
dotnet format doctorloan-api/src/WebUI/DoctorLoan.WebAPI/DoctorLoan.WebAPI.csproj
```

### Build và kiểm tra warnings

```bash
cd doctorloan-api
dotnet build --configuration Release
```

### Chạy code analysis

```bash
dotnet build --configuration Release \
  /p:RunAnalyzersDuringBuild=true \
  /p:EnableNETAnalyzers=true \
  /p:AnalysisLevel=latest
```

### Kiểm tra NuGet vulnerabilities

```bash
dotnet list doctorloan-api/DoctorLoan.sln package --vulnerable --include-transitive
```

### Generate OpenAPI Specification

```bash
cd doctorloan-api/src/WebUI/DoctorLoan.WebAPI
dotnet build --configuration Debug
# OpenAPI spec sẽ được generate tại wwwroot/api/specification.json
```

### Test API locally

```bash
# Start PostgreSQL (nếu chưa có)
docker run -d --name postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=doctorloan -p 5432:5432 postgres:14

# Run API
cd doctorloan-api/src/WebUI/DoctorLoan.WebAPI
dotnet run

# Test health check
curl http://localhost:5000/health

# Test Swagger
open http://localhost:5000/api
```

## 📊 Quality Report

Mỗi lần CI chạy, một **Comprehensive Code Quality & API Validation Report** được tạo tự động với:

### Code Quality Section
- Build Errors và Warnings count
- Code analysis warnings
- Code smells (TODO/FIXME, commented code)

### Security Section
- Total security issues
- Critical và High severity issues count
- Vulnerable packages
- Hardcoded secrets

### Performance Section
- Total performance issues
- Complexity findings
- Anti-patterns detected

### API Validation Section (API-specific)
- OpenAPI Specification status (Valid/Invalid)
- Total API Endpoints
- Endpoints with Documentation
- Missing Response Schemas
- API Configuration status
- Security Headers status
- Health Checks status

Xem report trong **Actions** → Workflow run → **Summary** tab.

## 🔍 Monitoring và Debugging

### Xem logs

- Vào **Actions** tab trong GitHub repository
- Click vào workflow run để xem chi tiết từng step
- Download artifacts để xem chi tiết logs:
  - `code-quality-logs` - Code quality analysis logs
  - `security-logs` - Security scanning logs
  - `performance-logs` - Performance analysis logs
  - `api-documentation` - API documentation và validation logs
  - `api-test-results` - API endpoint testing logs

### Xem GitHub Issues

- Vào **Issues** tab trong GitHub repository
- Filter theo labels:
  - `severity:critical` - Critical issues
  - `type:security` - Security issues
  - `type:performance` - Performance issues
  - `type:api` - API-specific issues

### Common issues

1. **Build fails**:
   - Kiểm tra `.csproj` dependencies
   - Kiểm tra .NET version (phải là 7.0.x)
   - Xem build logs để tìm lỗi cụ thể

2. **Formatting fails**:
   - Chạy `dotnet format doctorloan-api/DoctorLoan.sln` để tự động fix
   - Commit và push lại

3. **Security warnings**:
   - Review hardcoded secrets - di chuyển sang configuration
   - Update vulnerable packages
   - Fix security code analysis warnings

4. **Performance warnings**:
   - Refactor large files
   - Fix blocking async operations
   - Optimize inefficient code

5. **API Validation fails**:
   - Kiểm tra NSwag configuration
   - Đảm bảo controllers có XML documentation
   - Kiểm tra API configuration trong appsettings.json
   - Xem API validation logs để tìm lỗi cụ thể

6. **API Endpoint Testing fails**:
   - Kiểm tra PostgreSQL service container
   - Kiểm tra connection string
   - Kiểm tra JWT configuration
   - Xem API server logs

7. **Issues không được tạo**:
   - Kiểm tra GitHub permissions
   - Kiểm tra GITHUB_TOKEN có được set không
   - Xem logs trong create-issues job

## 📚 Best Practices

### Trước khi push code

1. **Format code**:
   ```bash
   dotnet format doctorloan-api/DoctorLoan.sln
   ```

2. **Build locally**:
   ```bash
   dotnet build doctorloan-api/DoctorLoan.sln --configuration Release
   ```

3. **Chạy code analysis**:
   ```bash
   dotnet build doctorloan-api/DoctorLoan.sln --configuration Release \
     /p:RunAnalyzersDuringBuild=true
   ```

4. **Kiểm tra security**:
   ```bash
   dotnet list doctorloan-api/DoctorLoan.sln package --vulnerable
   ```

5. **Generate và kiểm tra OpenAPI spec**:
   ```bash
   cd doctorloan-api/src/WebUI/DoctorLoan.WebAPI
   dotnet build --configuration Debug
   # Kiểm tra wwwroot/api/specification.json
   ```

6. **Test API locally**:
   ```bash
   dotnet run --project doctorloan-api/src/WebUI/DoctorLoan.WebAPI
   # Test health check, Swagger, etc.
   ```

7. **Review code smells**:
   - Fix TODO/FIXME comments
   - Xóa commented code
   - Extract constants

8. **Commit và push**:
   ```bash
   git add .
   git commit -m "Your commit message"
   git push origin your-branch
   ```

### Pull Request Workflow

1. Tạo branch từ main/develop
2. Code và commit changes
3. Push branch lên GitHub
4. Tạo Pull Request
5. CI sẽ tự động chạy và kiểm tra code
6. Review **Quality Report** trong PR
7. Review các **Issues** được tạo tự động
8. Fix issues nếu có
9. Chỉ merge khi CI pass ✅

### Code Quality Guidelines

1. **Naming Conventions**:
   - Classes, Methods, Properties: PascalCase
   - Parameters, Local variables: camelCase
   - Constants: PascalCase
   - Private fields: camelCase

2. **Code Style**:
   - Sử dụng `var` khi type rõ ràng
   - Prefer expression-bodied members cho simple properties
   - Sử dụng pattern matching
   - Null-checking với null-conditional operators

3. **Security Best Practices**:
   - Không hardcode secrets
   - Validate input
   - Use parameterized queries
   - Implement proper authentication/authorization
   - Keep packages updated
   - **API-specific**: Use JWT tokens, implement rate limiting, configure CORS properly

4. **Performance Best Practices**:
   - Use async/await cho I/O operations
   - Avoid blocking async operations
   - Use StringBuilder cho string concatenation trong loops
   - Optimize LINQ queries
   - Cache expensive operations
   - **API-specific**: Use async database operations, implement response caching

5. **Maintainability**:
   - Keep files < 500 lines
   - Keep methods focused và short
   - Apply SOLID principles
   - Write clear comments cho complex logic

### API Best Practices

1. **API Documentation**:
   - Thêm XML comments cho controllers và actions
   - Document request/response models
   - Document error responses
   - Document authentication requirements
   - Keep OpenAPI spec up-to-date

2. **API Security**:
   - Implement JWT authentication
   - Configure rate limiting
   - Configure CORS properly
   - Implement security headers
   - Validate all inputs
   - Use HTTPS in production

3. **API Design**:
   - Follow RESTful conventions
   - Use appropriate HTTP methods
   - Return appropriate HTTP status codes
   - Implement proper error handling
   - Use consistent response formats

4. **API Testing**:
   - Write unit tests cho controllers
   - Write integration tests cho API endpoints
   - Test authentication/authorization
   - Test error scenarios
   - Test rate limiting

5. **API Monitoring**:
   - Implement health checks
   - Log API requests/responses
   - Monitor API performance
   - Set up alerts cho errors

## 🎯 Gợi ý Tối Ưu Hóa Code

CI pipeline tự động phát hiện và gợi ý các cải tiến:

### Performance Optimizations

- **String operations**: Sử dụng StringBuilder thay vì string concatenation
- **LINQ**: Sử dụng `.ToList()` hoặc `.ToArray()` khi cần enumerate nhiều lần
- **Async operations**: Sử dụng async/await cho I/O operations
- **Caching**: Cache expensive operations
- **Memory**: Avoid large allocations, use object pooling nếu cần
- **API-specific**: Use async database operations, implement response caching

### Code Structure

- **Long methods**: Chia nhỏ methods > 50 lines
- **Long classes**: Chia nhỏ classes > 500 lines
- **Cyclomatic complexity**: Giảm complexity của methods
- **Duplication**: Extract common code vào methods/classes

### Security

- **Secrets management**: Sử dụng configuration hoặc secure storage
- **Input validation**: Validate tất cả user input
- **SQL injection**: Sử dụng parameterized queries
- **XSS**: Encode output
- **Package updates**: Keep packages updated để tránh vulnerabilities
- **API-specific**: Implement proper authentication, rate limiting, CORS

### Maintainability

- **Documentation**: Thêm XML comments cho public APIs
- **Naming**: Sử dụng descriptive names
- **Comments**: Thêm comments cho complex logic
- **Tests**: Viết unit tests cho business logic và API endpoints

### API-Specific Optimizations

- **Response size**: Minimize response payloads
- **Pagination**: Implement pagination cho list endpoints
- **Filtering**: Implement filtering và sorting
- **Caching**: Cache static data
- **Compression**: Enable response compression
- **Versioning**: Implement API versioning nếu cần

## 🔧 Tối ưu Workflow

Workflow được tối ưu với:

1. **Parallel Jobs**: Các jobs chạy song song để giảm thời gian
2. **Caching**: Cache NuGet packages để tăng tốc restore
3. **Artifacts**: Upload logs và findings để review sau
4. **Conditional Execution**: Một số steps chỉ chạy khi cần
5. **Timeout**: Set timeout cho mỗi job để tránh hang
6. **Service Containers**: Sử dụng PostgreSQL service container cho API testing

## 🎯 Next Steps

1. ✅ Push code lên bất kỳ branch nào để test CI
2. ✅ Xem kết quả trong Actions tab
3. ✅ Review Quality Report
4. ✅ Review GitHub Issues được tạo tự động
5. ✅ Fix các issues theo severity
6. ✅ Đảm bảo CI pass trước khi merge
7. ✅ Cải thiện code quality và API quality dựa trên gợi ý
8. ✅ Thêm unit tests và integration tests cho API endpoints
9. ✅ Cải thiện API documentation
10. ✅ Implement API versioning nếu cần

## 📖 Tài liệu tham khảo

- [.NET Code Analysis](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/overview)
- [StyleCop Analyzers](https://github.com/DotNetAnalyzers/StyleCopAnalyzers)
- [EditorConfig](https://editorconfig.org/)
- [GitHub Actions](https://docs.github.com/en/actions)
- [OWASP Top 10](https://owasp.org/www-project-top-ten/)
- [CWE Top 25](https://cwe.mitre.org/top25/)
- [OpenAPI Specification](https://swagger.io/specification/)
- [NSwag Documentation](https://github.com/RicoSuter/NSwag)
- [ASP.NET Core Web API Best Practices](https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-7.0)
- [REST API Design Best Practices](https://restfulapi.net/)

---

**Lưu ý**: CI pipeline này chạy trên **TẤT CẢ CÁC BRANCH** để đảm bảo code quality và API quality nhất quán trong toàn bộ project. Security, Performance và API validation issues sẽ được tự động tạo thành GitHub Issues với severity labels để dễ dàng tracking và fixing.

