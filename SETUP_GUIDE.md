# Quick Setup Guide for Ervilhinha

## 🚀 Quick Start

### Step 1: Database Setup ✅ DONE
The database has been created and migrations have been applied.

### Step 2: Set Up Your First Admin User

**Option A: Via User Secrets (Recommended for Development)**

1. Right-click on the project in Solution Explorer
2. Select "Manage User Secrets"
3. Add your Google credentials (if using Google login):

```json
{
  "Authentication": {
    "Google": {
      "ClientId": "YOUR_CLIENT_ID",
      "ClientSecret": "YOUR_CLIENT_SECRET"
    }
  },
  "AdminUser": {
    "Email": "your-email@example.com",
    "Password": "YourSecurePassword123!"
  }
}
```

**Option B: Manual Database Entry (Quick Start)**

1. Run the application (F5)
2. Click "Register" and create your first account
3. Stop the application
4. Open "SQL Server Object Explorer" in Visual Studio
5. Navigate to: (localdb)\MSSQLLocalDB → Databases → aspnet-Ervilhinha-* → Tables
6. Find the AspNetRoles table and note the Admin role Id
7. Find your user Id in AspNetUsers table
8. In AspNetUserRoles table, insert a new row:
   - UserId: (your user id)
   - RoleId: (admin role id)
9. Run the application again - you're now an admin!

### Step 3: Set Up Google Authentication (Optional)

#### Get Google OAuth Credentials:

1. Go to https://console.cloud.google.com/
2. Create a new project or select existing
3. Enable "Google+ API"
4. Go to "Credentials" → "Create Credentials" → "OAuth Client ID"
5. Application type: "Web application"
6. Add Authorized redirect URIs:
   - `https://localhost:7xxx/signin-google` (replace xxx with your port)
   - Check your launchSettings.json for the exact HTTPS port
7. Copy the Client ID and Client Secret

#### Update Configuration:

Either in `appsettings.json` (not recommended for production):
```json
"Authentication": {
  "Google": {
    "ClientId": "YOUR_CLIENT_ID.apps.googleusercontent.com",
    "ClientSecret": "YOUR_CLIENT_SECRET"
  }
}
```

OR in User Secrets (recommended):
1. Right-click project → Manage User Secrets
2. Add the same JSON structure there

### Step 4: Add Sample Data (Optional)

As an admin, you can now:

1. **Create Expense Categories**:
   - Go to "🏷️ Categories" in the navigation
   - Add categories like: "Baby Items", "Groceries", "Healthcare", "Utilities", etc.
   - Choose colors for each category

2. **Add Baby Items**:
   - Go to "👶 Baby Items"
   - Add items you need (crib, diapers, clothes, etc.)
   - Set priorities and estimated costs

3. **Track Expenses**:
   - Go to "💰 Expenses"
   - Start adding your expenses
   - View reports to see your spending

### Step 5: Invite Family Members

Share the application with your wife and family:

1. They can register their own accounts
2. You (as admin) can assign them the "User" role using the database method above
3. They'll be able to:
   - View and update baby items
   - Add and edit expenses
   - View reports
   - Mark items as purchased

---

## 🔧 Troubleshooting

### Port Issues
Check `Properties/launchSettings.json` for the correct ports your app is running on.

### Google Login Not Working
- Verify the redirect URI matches exactly what you configured in Google Console
- Make sure you're using HTTPS
- Check that Google+ API is enabled in your Google Cloud project

### Database Issues
If you need to reset the database:
```powershell
dotnet ef database drop
dotnet ef database update
```

### Can't Access Admin Features
Make sure your user is in the "Admin" role in the database (AspNetUserRoles table).

---

## 📱 Using the Application

### Baby Planning
1. **Add Items**: Click "Add New Item" on the Baby Items page
2. **Categorize**: Use built-in categories or create your own
3. **Set Priority**: 1 (highest) to 5 (lowest)
4. **Track Costs**: Add estimated and actual costs
5. **Mark as Purchased**: Click the checkmark when someone buys an item

### Expense Tracking
1. **Create Categories**: Admin creates categories first
2. **Add Expenses**: Everyone can add their expenses
3. **Filter & Search**: Use date range and category filters
4. **View Reports**: See breakdown by category and month

### Roles & Permissions
- **Admin**: Full access to everything
- **User**: Can view/edit items, add expenses, limited delete access

---

## 🎯 Next Steps

1. ✅ Set up your admin account
2. ✅ Configure Google login (optional)
3. ✅ Create expense categories
4. ✅ Add baby items
5. ✅ Invite family members
6. ✅ Start tracking expenses!

Enjoy planning for your new arrival! 👶💕
