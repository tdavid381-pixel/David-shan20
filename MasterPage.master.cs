using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace AgricultureManagementSystem
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        private readonly string s =
            ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString;

        public int CartCount
        {
            get
            {
                if (Session["farmer"] == null || Session["fid"] == null)
                {
                    return 0;
                }

                int farmerId;

                if (!int.TryParse(Session["fid"].ToString(), out farmerId))
                {
                    return 0;
                }

                using (SqlConnection con = new SqlConnection(s))
                {
                    string query =
                        "SELECT COUNT(*) FROM dbo.Cart WHERE FarmerID = @FarmerID";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@FarmerID", SqlDbType.Int).Value =
                            farmerId;

                        con.Open();

                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["farmer"] != null)
            {
                phGuestLinks.Visible = false;
                phFarmerLinks.Visible = true;

                string path = Request.Url.AbsolutePath.ToLowerInvariant();

                if (path.Contains("/farmer/") &&
                    !path.Contains("farmerlogin") &&
                    !path.Contains("farmerregister"))
                {
                    phSidebar.Visible = true;
                    phFarmerSidebarLinks.Visible = true;

                    mainContentCol.Attributes["class"] = "col-12 col-lg-10";
                }
            }
            else
            {
                phGuestLinks.Visible = true;
                phFarmerLinks.Visible = false;
            }
        }

        protected void lnkFarmerLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            Response.Redirect("~/Public/Default.aspx");
        }
    }
}
