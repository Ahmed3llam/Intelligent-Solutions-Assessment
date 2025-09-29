## Financing Leads Management API
This project is a layered **.NET Web API**.  
It manages financing leads, including creation, reading, searching, reviewing, and notifications (via Firebase Cloud Messaging).

---

## Project Structure

- **Domain**:  
  Contains core business entities, enums, and domain events.  
  Example: `FinancingLead`, `LeadReviewStatus`, `LeadReviewedDomainEvent`.

- **Application**:  
  Contains DTOs, commands, queries, and interfaces (Repositories, Notifications).  
  Uses **MediatR** for CQRS.

- **Infrastructure**:  
  Implements repository interfaces (EF Core), notifications (Firebase), and persistence (SQL Server).

- **API**:  
  Exposes REST endpoints for managing financing leads.

---

## Configuration

Update `appsettings.json`: 
- update ConnectionStrings (db name - user id - password) 
- update NotificationSettings (project ID - path of file that will be downloaded when getting the private key)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=Assessment;User Id=---;Password=--;TrustServerCertificate=true"
  },
  "NotificationSettings": {
    "ProjectId": "",
    "ServiceAccountPath": ""
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

---

## Run EF Core migrations:
- dotnet ef database update

---

## FinancingLead Entity Configuration

The `FinancingLead` entity is configured using `IEntityTypeConfiguration<FinancingLead>` inside `Infrastructure/Mapping/FinancingLeadConfiguration.cs`.

### Property Configuration
- **Id** → Primary key (GUID).
- **RowVersion** → Concurrency token (`IsRowVersion`).
- **Name** → Required, `nvarchar(100)`.
- **Email** → Required, `nvarchar(254)`.
- **PhoneE164** → Required, `nvarchar(16)`, stored in E.164 format for consistency.
- **TypeOfActivity** → Required, `nvarchar(100)`.
- **CommercialRegisterType** → Required, `nvarchar(50)`.
- **AnnualIncome** → Decimal with precision `(18,2)`.
- **Notes** → Optional, up to 500 characters.
- **CreatedAt** → Required `datetime`, stored in **UTC**.
- **ReviewedAt** → Nullable `datetime`, stored in **UTC**.
- **Status** → Enum converted to string, up to 20 characters.
- **ReviewReason** → Optional, up to 250 characters.
- **Events** → Domain events collection, ignored by EF (not persisted).

### Indexes
- **CreatedAt DESC** → `IX_Lead_CreatedAt_Desc` for fast sorting by latest leads.  
- **PhoneE164** → Unique index to ensure one lead per phone number.  
- **Email** → Unique index to ensure one lead per email.  
- **Composite (Name, Email, PhoneE164)** → `IX_Lead_ContactComposite` for efficient multi-field searching.  

### UTC Handling Strategy
- All `DateTime` values (`CreatedAt`, `ReviewedAt`) are stored in **UTC** to avoid timezone inconsistencies.  

---

## Application and Integration Justifications
A. **CQRS & Mapping**

  Projection Strategy: 
  
   - Read queries (GetFinancingLeadsQuery) use AsNoTracking() in the repository implementation to enhance read performance and reduce memory usage.
  
B. **Firebase Notification Integration**

  - Normalization Rule (FCM Topic): FCM topics do not allow the + symbol. The implemented normalization rule is:
Replace the leading + with plus- and convert to lowercase.

     Example: +15551234567 becomes plus-15551234567.

  - Notification Integration Trade-off: The INotificationClient is injected directly into the ReviewFinancingLeadHandler instead of using a complex domain event dispatcher mechanism.

---

## Endpoints
- **Base URL**: http://localhost:5000/api/financingleads
- **Create a Lead**: POST /api/financingleads
  
     -- **Request**:
    ```json
          {
              "name": "Ahmed",
              "email": "ahmed@any.com",
              "phoneE164": "+201234567890",
              "typeOfActivity": "Retail",
              "commercialRegisterType": "LLC",
              "preferredContactMethod": "Email",
              "annualIncome": 1000000.54,
              "notes": "Needs financing for expansion."
            }
   ```
    -- **Response (201 Created):**

  ```json
    {
      "id": "c95a74fa-14a3-4c5d-8b7e-6a12ef6a8fd1"
    }
  ```
- **Get Leads (with filtering & paging)**: GET /api/financingleads
 
    -- **Response**:
    ```json
      {
        "items": [
          {
            "id": "c95a74fa-14a3-4c5d-8b7e-6a12ef6a8fd1",
            "name": "Ahmed",
            "email": "ahmed@any.com",
            "phoneE164": "+201234567890",
            "status": "Pending",
            "createdAt": "2025-09-29T07:15:00Z",
            "typeOfActivity": "Retail",
            "commercialRegisterType": "LLC",
            "preferredContactMethod": "Email",
            "annualIncome": 1000000.54,
            "notes": "Needs financing for expansion."
          }
        ],
        "totalPages": 1,
      }
    ```

    -- **Filters supported**:
  
      - search + searchWith (Name, Email, Phone)
      - phoneStartsWith
      - from / to (CreatedAt range)
      - status (Pending, Accepted, Rejected)
      - Sorting: sortBy=name|status|createdAt&sortDir=asc|desc

- **Review a Lead**: POST /api/financingleads/review

    -- **Request**:
    ```json
      {
        "Id": "c95a74fa-14a3-4c5d-8b7e-6a12ef6a8fd1",
        "status": "Accepted",
        "reason": "Documents validated."
      }
   ```
    -- **Response (201 Created):**

  ```json
    {
      "decision": "Accepted",
      "reviewReason": "validated.",
    }
  ```

----

## Notifications (Firebase)

When a lead is reviewed with status Accepted, a Firebase Push Notification is dispatched.

- Payload:
```json
  {
    "to": "/topics/leads",
    "notification": {
      "title": "Lead Accepted",
      "body": "Your financing lead was accepted."
    }
  }
```

---

## Running Locally

- dotnet restore
- dotnet build
- dotnet run --project API

---

## Notes

- All timestamps are stored and returned in UTC.
- Domain events are published when important business actions occur (e.g., Lead Reviewed).
- Concurrency is handled with EF Core RowVersion.

---
## Thanks
