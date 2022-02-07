Feature: HealthCheck
	Health Checks

@healthcheck
Scenario: Successful Health Check
	Given all dependencies are healthy
	When a health check request is performed
	Then an OK health check response is returned