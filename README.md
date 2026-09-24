# DiscountMate

I initially wanted to create my DiscountMate extension but realised it was above my current skill level. So I created a simpler application to compare prices between two stores.

Users can search products, sort prices, filter by protein and add or remove items from the cart. The cart calculates the total.

Prices and nutritional values use a random generator with a fixed seed so they stay the same between runs.

I started with the product list and added the other functions gradually. Then I added 12 tests and set up the Jenkins pipeline one stage at a time.

The seven stages are Build, Test, Code Quality, Security, Deployment, Release and Monitoring.

I used .NET for the build, .NET with xUnit for tests, SonarQube for code quality and NuGet Audit for package security checks. Docker runs the test and released copies. Uptime Kuma checks the released application every 60 seconds.



Local addresses (reminder for me)
- Development: http://localhost:5144
- Test: http://localhost:5081
- Released application: http://localhost:5082
- Jenkins: http://localhost:8080
- SonarQube: http://localhost:9000
- Uptime Kuma: http://localhost:3001

