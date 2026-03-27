# Ervilhinha - Baby Planning & Finance Management App 👶💰

A comprehensive web application to help you and your family prepare for a new baby while managing your finances effectively.

## Features

### 👶 Baby Planning
- Track all items needed for your baby
- Organize by categories (Clothing, Nursery, Feeding, etc.)
- Set priorities for each item
- Mark items as purchased and track who bought them
- Estimate and actual cost tracking
- Collaborative - family and friends can help update the list

### 💰 Expense Tracking
- Record all your expenses
- Categorize expenses with custom categories
- Filter by date range and category
- Track who created each expense
- Add notes to expenses

### 📊 Financial Reports
- View total spending
- Break down expenses by category
- Monthly spending analysis
- Visual reports to understand spending patterns

### 👥 User Roles
- **Admin Users**: Full access to create, edit, delete items and manage categories
- **Normal Users**: Can view and update items, add expenses, but limited delete permissions

### 🔐 Easy Authentication
- Sign in with Google account
- Traditional email/password registration
- Secure authentication with ASP.NET Core Identity

## Setup Instructions

### Prerequisites
- Visual Studio 2022 or later
- .NET 8 SDK
- SQL Server LocalDB (comes with Visual Studio)

### Configuration

#### 1. Google Authentication Setup (Optional but Recommended)

1. Go to [Google Cloud Console](https://console.cloud.google.com/)
2. Create a new project or select an existing one
3. Enable the Google+ API
4. Go to "Credentials" and create OAuth 2.0 Client ID
5. Configure the OAuth consent screen
6. Add authorized redirect URIs:
   - `https://localhost:5001/signin-google` (for HTTPS)
   - `http://localhost:5000/signin-google` (for HTTP)
7. Copy the Client ID and Client Secret
8. Update `appsettings.json`:
   ```json
   "Authentication": {
     "Google": {
       "ClientId": "YOUR_GOOGLE_CLIENT_ID",
       "ClientSecret": "YOUR_GOOGLE_CLIENT_SECRET"
     }
   }
   ```

#### 2. Database Setup

Open Package Manager Console in Visual Studio and run:

```powershell
Add-Migration InitialCreate
Update-Database
```

#### 3. Create Admin User

After running the application:
1. Register a new user account
2. Stop the application
3. Open SQL Server Object Explorer
4. Find your database: `aspnet-Ervilhinha-*`
5. Manually assign the Admin role to your user in the `AspNetUserRoles` table

**OR** you can add seed data by modifying `Program.cs` to create an admin user automatically.

### Running the Application

1. Open the solution in Visual Studio
2. Press F5 or click "Start"
3. The application will open in your default browser
4. Register a new account or sign in with Google

## Usage Guide

### For First-Time Users

1. **Register/Sign In**: Create an account or use Google sign-in
2. **Set Up Categories**: Admins should first create expense categories (Groceries, Baby Items, Healthcare, etc.)
3. **Add Baby Items**: Start adding items you need for your baby
4. **Track Expenses**: Record your spending to keep track of your budget
5. **View Reports**: Check the Reports page to see spending patterns

### For Family Members

1. Ask the admin to create an account for you or register yourself
2. View the baby items list
3. Mark items as purchased when you buy them
4. Add expenses when you make purchases

## Project Structure

```
Ervilhinha/
├── Controllers/
│   ├── BabyItemsController.cs      # Baby items management
│   ├── ExpensesController.cs        # Expense tracking
│   ├── ExpenseCategoriesController.cs # Category management (Admin only)
│   └── HomeController.cs
├── Models/
│   ├── BabyItem.cs                  # Baby item model
│   ├── Expense.cs                   # Expense model
│   └── ExpenseCategory.cs           # Category model
├── Views/
│   ├── BabyItems/                   # Baby items views
│   ├── Expenses/                    # Expense views
│   ├── ExpenseCategories/           # Category views
│   └── Shared/
├── Data/
│   ├── ApplicationDbContext.cs      # EF Core context
│   └── Migrations/
└── Program.cs                       # Application configuration
```

## Technologies Used

- **ASP.NET Core 8.0** - Web framework
- **Entity Framework Core** - ORM
- **SQL Server** - Database
- **ASP.NET Core Identity** - Authentication & Authorization
- **Bootstrap 5** - UI framework
- **Razor Pages** - View engine

## Security Features

- Role-based authorization
- Anti-forgery tokens
- Secure password hashing
- External authentication (Google)
- HTTPS enforcement

## Future Enhancements

- Email notifications for important events
- Baby countdown timer
- Shared shopping lists
- Budget planning and alerts
- Export reports to PDF
- Mobile app
- Photo attachments for items

## Support

For issues or questions, please contact the administrator or create an issue in the project repository.

## License

This project is for personal use.

---

Made with ❤️ for growing families
