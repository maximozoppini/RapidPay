# RapidPay Authorization System

## Features

### Card Management Module
- **Create Card**: Generate a new 15-digit card.
- **Pay**: Make payments using the created card, with balance updates.
- **Get Card Balance**: Retrieve the current balance of a card.

### Payment Fees Module
- **Dynamic Payment Fee Calculation**: Simulates the Universal Fees Exchange (UFE) system, which updates the fee every hour with a randomly generated multiplier between 0 and 2. This logic is implemented as a thread-safe Singleton.

## Getting Started

Follow these steps to set up and run the project:

### Prerequisites
1. **Database**: This project assumes SQL Server, but you can modify the connection string for other databases.

### Setup Instructions
1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd <repository-folder>
   ```
2. Open the solution in your preferred IDE (e.g., Visual Studio or Visual Studio Code).

3. Update the connection string in the `appsettings.json` file to point to your database:
   ```json
   "ConnectionStrings": {
       "DefaultConnection": "YourDatabaseConnectionStringHere"
   }
   ```

4. Run the Web API:
   ```bash
   dotnet run
   ```

5. The API will automatically:
   - Create the necessary database tables.


## Author
Developed by **Maximo Zoppini**.
