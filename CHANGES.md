# Changes

## Trimmed + semester 5 style version
**Pages removed:** all Admin pages, Gallery, About, Services, Fertilizers, Smart Crop Advisor,
Farmer Orders / Feedback / Profile, and the weather widget (`Controls/`, `Secrets.config`).
**Pages kept (15):** Default, Products, Cart, Equipment, EquipmentCart, Contact, FarmerLogin,
FarmerRegister, FarmerDashboard, AddCrop, ViewCrop, AddProduct, ManageProducts, ApplyEquipment, Payment.

**Rewritten in the notebook style (all pages):**
- `getcon()` helper + `SqlConnection con / SqlCommand cmd / SqlDataAdapter da / DataSet ds` fields
  (no `using` blocks, no parameters, no transactions, no base page classes).
- `Repeater` -> `DataList` (Products, Equipment); cart pages -> `GridView` with Edit / Update / Cancel / Delete.
- Connection string renamed `AgricultureDBConnection` -> `dbcon`.
- Login now stores `Session["farmer"]` (email), `Session["fid"]`, `Session["fname"]`.
- Carts moved from `Session` to the database: new tables `Cart` and `EquipCart`
  (`Database/Migrations/11_Cart_Tables.sql`). A farmer must be logged in to use them.
- `MasterPage.master.cs` now shows the farmer menu / sidebar when logged in and handles Logout.
- `Payment.aspx` handles equipment applications only (the fertilizer / product branches were never used).
- `ApplyEquipment.aspx` works out the price from the database when the form is submitted.

**Database cleanup:**
- `AgricultureDB.sql`: removed the tables Admin, Fertilizers, Feedback (and their seed data); added `Cart` and `EquipCart`.
  It still drops the old tables first, so it can be re-run on an old database.
- `02_Schema_Upgrades.sql`: removed the FertilizerOrders table.
- Deleted the crop gallery scripts (05-09). `11_Cart_Tables.sql` is only for databases made before the cart tables existed.
- New `12_OPTIONAL_Remove_Unused_Tables.sql` drops the unused tables from an existing database (this deletes their data).
