Good Morning/Evening/Afternoon

To ensure the app runs seamlessly please, after adding migrations and updating the database, please run the MenlynMewsSeedRecords FINAL v.10.sql. 

The following record has been seeded to grant Manager Access

 var user = new ApplicationUser
 {
     Id = "ammarManager",
     UserName = "TheManager", //USERNAME
     NormalizedUserName = "THEMANAGER",
     Email = "########", //For Security Purposes, the email is my student number and the tuks domain email address
     EmailConfirmed = true,
     TwoFactorEnabled = true
 };

 user.PasswordHash = hasher.HashPassword(user, "Password@123"); //PASSWORD

And whem the seed record script is run it will be successfully be linked to an Employee record ensuring complete control
