using DataAccessLayer.DataUtilities;
using DataAccessLayer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class Repository
    {
        private readonly DataUtility _dataUtility;
        public Repository(DataUtility dataUtility)
        {
            _dataUtility = dataUtility; 
        }

        public string InsertData(FormModel model)
        {
            string message = string.Empty;
            try
            {
                string ProfileImage = "";
                if (model.ProfilePath != null)
                {
                    ProfileImage = Path.GetFileName(model.ProfilePath.FileName);
                    var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    var filepath = Path.Combine(directoryPath, ProfileImage);

                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        model.ProfilePath.CopyTo(stream);
                    }
                }


                string skillAdd = "";
                if (model != null && model.Skills != null && model.Skills.Count > 0)
                {
                    skillAdd = string.Join(",", model.Skills);
                }

                SqlParameter[] para =
                {
                    new SqlParameter("@Name",model.Name),
                    new SqlParameter("@Email",model.Email),
                    new SqlParameter("@ProfilePath",ProfileImage),
                    new SqlParameter("@Gender",model.Gender),
                    new SqlParameter("@Qualification",model.Qualification),
                    new SqlParameter("@Skills",skillAdd),
                    new SqlParameter("@Message",model.Message),
                    new SqlParameter("@MobileNumber",model.MobileNumber),
                };

                int result = _dataUtility.ExecuteSql("sp_InsertFormData", para);
                if (result > 0)
                {
                    message = "Added Successfully";
                }
                else
                {
                    message = "Try again";
                }
                return message;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<FormModel> GetAllFormData()
        {
            List<FormModel> formDataList = new List<FormModel>();
            try
            {
                DataTable dataTable = _dataUtility.GetDataTable("sp_GetAllFormData");

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        FormModel model = new FormModel();

                        model.Id = row["Id"] != DBNull.Value ? Convert.ToInt32(row["Id"]) : 0;
                        model.Name = row["Name"] != DBNull.Value ? Convert.ToString(row["Name"]) : "";
                        model.Email = row["Email"] != DBNull.Value ? Convert.ToString(row["Email"]) : "";
                        model.MobileNumber = row["MobileNumber"] != DBNull.Value ? Convert.ToInt64(row["MobileNumber"]) : 0;
                        model.Gender = row["Gender"] != DBNull.Value ? Convert.ToString(row["Gender"]) : "";
                        model.Qualification = row["Qualification"] != DBNull.Value ? Convert.ToString(row["Qualification"]) : "";
                        model.Message = row["Message"] != DBNull.Value ? Convert.ToString(row["Message"]) : "";
                        model.ProfilePathStr = row["ProfilePath"] != DBNull.Value ? Convert.ToString(row["ProfilePath"]) : "";

                        formDataList.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return formDataList;
        }
        public FormModel GetFormModelById(int Id)
        {
            FormModel formModel = new FormModel();
            try
            {
                SqlParameter[] para =
                { 
                    new SqlParameter("@Id",Id)
                };

                DataTable dataTable = _dataUtility.GetDataTable("sp_GetDataByID", para);

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        formModel.Id = row["Id"] != DBNull.Value ? Convert.ToInt32(row["Id"]) : 0;
                        formModel.Name = row["Name"] != DBNull.Value ? Convert.ToString(row["Name"]) : "";
                        formModel.Email = row["Email"] != DBNull.Value ? Convert.ToString(row["Email"]) : "";
                        formModel.MobileNumber = row["MobileNumber"] != DBNull.Value ? Convert.ToInt64(row["MobileNumber"]) : 0;
                        formModel.Gender = row["Gender"] != DBNull.Value ? Convert.ToString(row["Gender"]) : "";
                        formModel.Qualification = row["Qualification"] != DBNull.Value ? Convert.ToString(row["Qualification"]) : "";
                        formModel.Message = row["Message"] != DBNull.Value ? Convert.ToString(row["Message"]) : "";
                        formModel.ProfilePathStr = row["ProfilePath"] != DBNull.Value ? Convert.ToString(row["ProfilePath"]) : "";
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return formModel;
        }
        public string UpdateData(FormModel model)
        {
            string message = string.Empty;
            try
            {
                string ProfileImage = "";
                if (model.ProfilePath != null)
                {
                    ProfileImage = Path.GetFileName(model.ProfilePath.FileName);
                    var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", ProfileImage);

                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        model.ProfilePath.CopyTo(stream);
                    }
                }
                else if(model.ProfilePathStr != null)
                {
                    ProfileImage = model.ProfilePathStr;
                }

                string skillAdd = "";
                if (model != null && model.Skills != null && model.Skills.Count > 0)
                {
                    skillAdd = string.Join(",", model.Skills);
                }

                SqlParameter[] para =
                {
                    new SqlParameter("@Id",model.Id),
                    new SqlParameter("@Name",model.Name),
                    new SqlParameter("@Email",model.Email),
                    new SqlParameter("@ProfilePath",ProfileImage),
                    new SqlParameter("@Gender",model.Gender),
                    new SqlParameter("@Qualification",model.Qualification),
                    new SqlParameter("@Skills",skillAdd),
                    new SqlParameter("@Message",model.Message),
                    new SqlParameter("@MobileNumber",model.MobileNumber),
                };

                int result = _dataUtility.ExecuteSql("sp_UpdateFormData", para);

                if (result > 0)
                {
                    message = "Added Successfully";
                }
                else
                {
                    message = "Try again";
                }
                return message;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public string DeleteData(int Id)
        {
            string message = string.Empty;
            try
            {
                SqlParameter[] para =
                {
                    new SqlParameter("@Id",Id)
                };

                int result = _dataUtility.ExecuteSql("sp_DeleteFormData", para);

                if (result > 0)
                {
                    message = "Added Successfully";
                }
                else
                {
                    message = "Try again";
                }
                return message;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
