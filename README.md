# Car Auction Management System

Car Auction Management System is a API that allows users to manage vehicle auctions. It provides functionality to add vehicles, start and manage auctions, place bids, and close auctions.

## Technologies Used

- .NET Core 8.0: For creating the backend API.
- Entity Framework Core: For database interaction (MySQL).
- MySQL: The relational database used for data storage.
- Swagger: To generate interactive API documentation.
- XUnit: For unit testing.
- AutoMapper: To map between DTOs and Entities.

## Decisions and Assumptions

When I started, I considered three ways to architect this API. To clearly demonstrate my point, I will show all the pros and cons of my thoughts.

### DTO Directly Converted to Entity (Chosen Approach)
<img width="745" alt="Screenshot 2024-10-03 at 13 54 54" src="https://github.com/user-attachments/assets/f73bc012-8c1c-4a22-aad3-17ce3375391b">

#### Pros:
- Scalability: You can easily add new properties for vehicle types without modifying the base structure. It’s more flexible for future growth.
- Simplicity: Direct mapping between DTOs and entities reduces complexity and overhead from multiple conversions.
- Unified Access: Retrieving all vehicles is efficient as they’re all in the same database and accessible through one endpoint.
#### Cons:
- Manual Property Management: You need to manually validate and manage properties, ensuring the right fields are set based on the vehicle type.

### Separate Endpoints and Databases for Each Vehicle Type
<img width="753" alt="Screenshot 2024-10-03 at 13 40 55" src="https://github.com/user-attachments/assets/dc5389e4-7289-44a9-90ab-61a408ff5dad">

#### Pros:
- Simplicity: Each vehicle type is handled independently, making it straightforward to manage specific vehicle-related logic.
- Clear Separation: Easier to enforce constraints and rules for each vehicle type.
#### Cons:
- Multiple Requests: When retrieving all vehicles, you would need to make multiple API requests (one per vehicle type), which can be inefficient.
- Harder to Scale: Adding a new vehicle type requires setting up a new database, endpoint, and related logic, making it cumbersome for growth.

### Base Vehicle Class with Subclasses for Each Type
<img width="742" alt="Screenshot 2024-10-03 at 13 52 45" src="https://github.com/user-attachments/assets/2d1f8851-70cb-4abf-b4e8-359e38f665f1">

#### Pros:
- Inheritance Flexibility: You can encapsulate shared properties in the base class and extend vehicle-specific logic in the subclasses.
#### Cons:
- Complexity in Mapping: You need to convert between DTOs, domain types, and entities, which adds unnecessary complexity.
- Performance Overhead: The conversion logic and mapping can introduce performance bottlenecks.
