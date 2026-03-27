# 📸 Quick Invoice Scanner Guide

## 🚀 Upload an Invoice in 3 Steps

### Step 1: Upload
1. Click **"📄 Invoices"** in menu
2. Click **"📸 Upload Invoice"**
3. Take photo OR select file
4. Click **"Upload & Process"**

### Step 2: Auto-Processing
- AI extracts vendor, amount, date
- Expense created automatically
- Status: "Needs Review" if uncertain

### Step 3: Review & Approve
1. Click **"Review"** button
2. Verify/fix any errors
3. Select expense category
4. Click **"✓ Approve & Save"**

**Done!** Expense is now in your system! 💰

---

## 💡 Tips for Best Results

| ✅ DO | ❌ DON'T |
|------|---------|
| Good lighting | Shadows/dark photos |
| Flat receipt | Crumpled paper |
| Capture all text | Cut off edges |
| Clear focus | Blurry images |
| Straight angle | Tilted/skewed |

---

## 📊 Invoice Status Meanings

- 🔵 **Pending**: Just uploaded
- 📝 **Processed**: AI extraction done
- ⚠️ **Needs Review**: Check for errors
- ✅ **Reviewed**: You approved it
- 💰 **Expense Created**: Linked to expense

---

## 🔧 Configuration (Optional)

### Without Azure (Default):
- Uses mock data
- Great for testing
- No setup needed
- Shows how it works

### With Azure (Real AI):
```json
// User Secrets
{
  "AzureFormRecognizer": {
    "Endpoint": "https://your-resource.cognitiveservices.azure.com/",
    "ApiKey": "YOUR_KEY"
  }
}
```

**Get Azure Free Tier**: 500 pages/month at [portal.azure.com](https://portal.azure.com)

---

## 🎯 Common Scenarios

### Grocery Receipt:
1. Upload receipt photo
2. AI finds store name + total
3. Review, assign "Groceries" category
4. Approve → Done!

### Baby Store Receipt:
1. Upload receipt
2. AI extracts data
3. Assign "Baby Items" category
4. See in reports immediately!

### Restaurant Bill:
1. Snap photo of receipt
2. AI reads amount + date
3. Categorize as "Dining"
4. Track dining expenses!

---

## 📱 Mobile Usage

- Works great on phones!
- Use camera directly
- Take photo in-app
- Upload immediately
- Review on any device

---

## ⚡ Pro Tips

1. **Batch Upload**: Upload multiple receipts at once
2. **Review Later**: Expenses created even if you don't review immediately
3. **Original Reference**: Keep invoice images for proof
4. **Category Setup**: Create categories before uploading
5. **Regular Review**: Check unreviewed invoices weekly

---

## 🐛 Troubleshooting

**Q: AI got amount wrong?**
- A: Use Review page to correct it!

**Q: Vendor name missing?**
- A: Add it manually in Review

**Q: Upload failed?**
- A: Check file type (JPG, PNG, PDF only)

**Q: Can't assign category?**
- A: Create categories first (Admin → Categories)

---

## 🎊 What Happens After Review?

1. ✅ Expense updated with correct data
2. ✅ Category assigned
3. ✅ Appears in expense list
4. ✅ Included in reports
5. ✅ Affects charts immediately
6. ✅ Invoice marked as "Reviewed"

---

**That's it! Start uploading invoices now!** 📸💚
