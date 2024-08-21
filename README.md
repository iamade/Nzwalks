**Monolith architecture**

**Running Migrations for Auth DB context**
```
dotnet ef migrations add CreatingAuthDatabase --context "NZWalksAuthDbContext"
```

**Update Auth Database**
```
dotnet ef database update --context "NZWalksAuthDbContext"
```

