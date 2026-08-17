# ApexLegendsTracker Cloud-Native Modernization Plan

## 1. Product vision
This project should act as a showcase for modern cloud-native engineering: resilient API communication, strong operational observability, autoscaling, production-grade quality gates, and a clean path to AWS deployment.

The current repo is a Blazor WebAssembly client, so the project should evolve into a full stack pattern:
- Frontend: Blazor UI
- API: ASP.NET Core backend service
- Data access: managed persistence and optional caching
- Platform: Kubernetes on AWS
- Observability: OpenTelemetry + Prometheus + Grafana

## 2. Current repo assessment
The existing frontend already exposes a clear contract boundary:
- `ApiBaseUrl` configuration value
- `HttpClient` injection in `Program.cs`
- `ApexTrackerApiClient.GetPlayerAsync` for `GET /api/v1/players/{platform}/{playerName}`

This is a solid starting point for a cloud-native showcase because the app is already structured around an explicit service boundary and dependency injection.

## 3. Target architecture

### Application architecture
```text
Browser
  -> ALB / Ingress
      -> Blazor app (frontend container)
      -> API service (backend container)
          -> optional Redis cache
          -> managed database / data provider
```

### AWS footprint
- EKS cluster
- Application Load Balancer for ingress
- ECR for container images
- IAM + Secrets Manager / KMS for secrets
- CloudWatch integration for logs and metrics
- Optional RDS or DynamoDB depending on data model
- Optional ElastiCache Redis for request-level caching

### Recommended platform stack
- Kubernetes: EKS
- Container runtime: Docker
- Service mesh: optional, not required for MVP
- ingress: ALB + Kubernetes ingress controller
- CI/CD: GitHub Actions + ECR + Helm or Kustomize
- IaC: Terraform

## 4. Resilience and scalability strategy
The showcase should visibly demonstrate production patterns, not just a simple app.

### Recommended patterns
- Retry with exponential backoff and jitter
- HTTP timeouts
- Circuit breaker for upstream failures
- Bulkhead isolation for critical calls
- Rate limiting and request coalescing
- Health probes and readiness checks
- HPA for horizontal scaling
- Queue-based async processing for non-interactive work

### In .NET
Use Polly or built-in resilience patterns for the API client.
Key behaviors:
- short timeout on external calls
- retry on transient 5xx and network failures
- backoff schedule with jitter
- fallback or graceful degraded UI state

## 5. Observability strategy
The best free/open source option for this kind of showcase is:
- OpenTelemetry for tracing, metrics, and logs
- Prometheus for metrics collection
- Grafana for dashboards
- Loki or OpenSearch for logs if needed
- EKS node/application metrics through kube-state-metrics

### Suggested stack
- App instrumentation: OpenTelemetry SDK in .NET
- Trace export: OTLP to a collector
- Metrics: Prometheus scraping via service monitors
- Dashboards: Grafana
- Alerts: Grafana alert rules or CloudWatch alarms

### Alternative SaaS option
- New Relic free tier for quick setup and easier demo-friendly dashboards
- Better for a short-term showcase, but open source is more reusable and showcases engineering depth

### Haystack angle
If you want an AI/data tooling component, you can pivot the platform to include a Haystack pipeline for enrichment, retrieval, or AI-assisted player analysis. Keep it optional. The cloud-native story should remain strong even if Haystack is not part of the first milestone.

## 6. Quality practices
This project should visibly emphasize engineering quality.

### Quality gates
- Unit tests for API client logic
- Integration tests for HTTP client behavior
- Contract tests for backend API payloads
- Load/performance tests with k6 or NBomber
- Container image scanning
- Dependency vulnerability scanning
- Static analysis and formatting checks in CI

### CI/CD pipeline
1. Build and test on PR
2. Run static analysis
3. Build Docker image
4. Scan image for vulnerabilities
5. Push to ECR
6. Deploy to dev EKS environment
7. Run smoke tests after deployment

## 7. Recommended MVP milestone plan

### Phase 1: foundation
- Add .NET backend API service with the same route contract
- Implement resilient HTTP client behavior in the frontend
- Add structured logging and health endpoints
- Add Prometheus and OpenTelemetry instrumentation

### Phase 2: AWS deployment
- Dockerize both services
- Build Helm charts or Kustomize manifests
- Deploy to EKS with ALB ingress
- Add autoscaling and managed secrets configuration

### Phase 3: production showcase polish
- Add Grafana dashboards for latency, error rate, saturation, and pod health
- Add alert rules for 5xx spikes and high p95 latency
- Run performance tests and tuning passes
- Add a clean README and architecture diagram for demos

## 8. Testing strategy
Use these layers:
- Unit tests: deterministic client behaviors and resilience logic
- Integration tests: API contract matching and server response validation
- Load tests: repeated requests, concurrency, retry stress
- Chaos / resilience tests: failure injection, degraded network, timeouts
- E2E tests: browser and UI flow validation

### Suggested tools
- xUnit / NUnit for .NET tests
- Playwright for browser tests
- k6 or NBomber for performance/load testing
- Docker Compose for local end-to-end testing before K8s

## 9. Recommended first implementation steps
1. Create a backend API repo or service in the same solution structure.
2. Add a shared contract model for player lookup responses.
3. Strengthen `ApexTrackerApiClient` with timeout, retry, and fallback handling.
4. Add OpenTelemetry instrumentation across the frontend and API.
5. Add Dockerfile, Helm chart, and Terraform skeleton.
6. Deploy to a dev EKS environment and validate traces and metrics.

## 10. Final recommendation
The strongest “showcase” stack for this project is:
- .NET API + Blazor frontend
- Docker containers
- EKS on AWS
- OpenTelemetry + Prometheus + Grafana
- Terraform for infrastructure
- GitHub Actions for CI/CD
- Polly for resiliency
- k6/Playwright for quality and performance assurance

This combination demonstrates scale, observability, failure handling, and production-minded engineering without requiring a heavy enterprise stack.
