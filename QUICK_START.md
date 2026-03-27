# 🎯 Ervilhinha - Quick Reference

## First-Time Setup (3 Easy Steps!)

### 1️⃣ Configure Admin User (Right-click project → Manage User Secrets)
```json
{
  "AdminUser": {
    "Email": "youremail@example.com",
    "Password": "SecurePassword123!"
  }
}
```

### 2️⃣ Run the Application
Press **F5** in Visual Studio

### 3️⃣ Login
- Your admin account will be created automatically
- Login with the email and password you configured

---

## 🔑 Default Features

### For Everyone (After Login)
- ✅ View and add baby items
- ✅ Mark items as purchased
- ✅ Add and edit expenses
- ✅ View expense reports
- ✅ Filter expenses by date and category

### Admin Only
- 👑 Manage expense categories
- 👑 Delete any items
- 👑 Full access to all features

---

## 🚀 Common Tasks

### Adding a Baby Item
1. Click "👶 Baby Items"
2. Click "➕ Add New Item"
3. Fill in details (name, category, priority)
4. Click "Save"

### Tracking an Expense
1. Click "💰 Expenses"
2. Click "➕ Add New Expense"
3. Enter description, amount, date, category
4. Click "Save"

### Creating a Category (Admin)
1. Click "🏷️ Categories"
2. Click "➕ Add New Category"
3. Enter name, description, choose color
4. Click "Save"

### Viewing Reports
1. Click "📊 Reports"
2. See total spending, breakdown by category, monthly totals

---

## 🔐 Google Login Setup (Optional)

### Quick Steps:
1. Go to https://console.cloud.google.com
2. Create project → Enable Google+ API → Create OAuth Client ID
3. Add redirect URI: `https://localhost:YOUR_PORT/signin-google`
4. Add to User Secrets:
```json
{
  "Authentication": {
    "Google": {
      "ClientId": "YOUR_CLIENT_ID",
      "ClientSecret": "YOUR_SECRET"
    }
  }
}
```

---

## 👥 Adding Family Members

### Easy Way:
1. Share the application URL
2. They register their own account
3. They automatically get "User" role
4. They can start helping immediately!

### Admin Assignment (if needed):
1. Open SQL Server Object Explorer
2. Find AspNetUsers → locate their UserId
3. Find AspNetRoles → locate RoleId for "Admin" or "User"
4. Add row to AspNetUserRoles with UserId and RoleId

---

## 💡 Pro Tips

### Baby Planning
- Use Priority 1 for must-haves, 5 for nice-to-haves
- Add estimated costs to track your budget
- Let family mark items as purchased to avoid duplicates

### Expense Tracking
- Create categories first (Baby Items, Groceries, Healthcare, etc.)
- Add expenses regularly to keep accurate records
- Use the Reports page to spot spending trends

### Security
- Keep User Secrets secure - never commit them to source control
- Use strong passwords
- Only give Admin role to trusted family members

---

## ⚡ Keyboard Shortcuts

- **F5** - Run application
- **Ctrl+F5** - Run without debugging
- **Shift+F5** - Stop debugging

---

## 🆘 Need Help?

### Check the logs
- View → Output → Show output from: Web Server

### Reset database
```powershell
dotnet ef database drop
dotnet ef database update
```

### Common Issues
- **Can't login?** Check User Secrets configuration
- **Google login fails?** Verify redirect URI matches your port
- **Not an admin?** Check User Secrets or database role assignment

---

**Remember**: The app creates roles automatically on first run!
Just configure your admin user in secrets and you're ready to go! 🎉
