import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminhomeComponent } from './admin/adminhome/adminhome.component';
//Actor Home Components
import { AgenthomeComponent } from './agent/agenthome/agenthome.component';
import { ClienthomeComponent } from './client/clienthome/clienthome.component';
import { CompanyhomeComponent } from './company/companyhome/companyhome.component';
//Login Components
import { LoginComponent } from './login/login.component';
//Pages Components
import { AboutComponent } from './pages/about/about.component';
import { ContactComponent } from './pages/contact/contact.component';
import { ErrorComponent } from './pages/error/error.component';
import { FeaturesComponent } from './pages/features/features.component';
import { HomeComponent } from './pages/home/home.component';
import { RegisterComponent } from './register/register.component';
import { ProfileComponent } from './profile/profile.component';
import { PolicytypesComponent } from './admin/policytypes/policytypes.component';
import { UserlistComponent } from './admin/userlist/userlist.component';
import { AdminViewComponent } from './admin/view/view.component';
import { PoliciesComponent } from './company/policies/policies.component';
import { AgentViewComponent } from './agent/view/view.component';
import { CompanyViewComponent } from './company/view/view.component';
import { ClientpoliciesComponent } from './agent/clientpolicies/clientpolicies.component';
import { CategoryComponent } from './agent/category/category.component';
import { ClientViewComponent } from './client/view/view.component';
import { CPoliciesComponent } from './client/policies/policies.component';
import { ReportComponent } from './report/report.component';
import { NomineeComponent } from './client/nominee/nominee.component';
import { DetailsComponent } from './client/details/details.component';
import { PaymentComponent } from './client/payment/payment.component';
import { PenaltyComponent } from './client/penalty/penalty.component';
import { DenialComponent } from './pages/denial/denial.component';


const routes: Routes = [

  { path: 'Home', title: 'SmartShield | Home', component: HomeComponent },
  { path: 'About', title: 'SmartShield | About', component: AboutComponent },
  { path: 'Contact', title: 'SmartShield | Contact', component: ContactComponent },
  { path: 'Features', title: 'SmartShield | Features', component: FeaturesComponent },
  //Home Paths
  { path: 'Home/Client', title: 'SmartShield | Client Home', component: ClienthomeComponent },
  { path: 'Home/Agent', title: 'SmartShield | Agent Home', component: AgenthomeComponent },
  { path: 'Home/Company', title: 'SmartShield | Company Home', component: CompanyhomeComponent },
  { path: 'Home/Admin', title: 'SmartShield | Admin Home', component: AdminhomeComponent },
  //Admin
  { path: 'Admin/PolicyTypes', title: 'SmartShield | Admin | Policy Types', component: PolicytypesComponent },
  { path: 'Admin/UserList', title: 'SmartShield | Admin | User List', component: UserlistComponent },
  { path: 'Admin/View/:choice', title: 'SmartShield | Admin | View', component: AdminViewComponent },
  //Company
  { path: 'Company/Policies', title: 'SmartShield | Company | Policies', component: PoliciesComponent },
  { path: 'Company/View/:choice', title: 'SmartShield | Company | View', component: CompanyViewComponent },
  //Agent
  { path: 'Agent/ClientPolicies', title: 'SmartShield | Agent | Client Policies', component: ClientpoliciesComponent },
  { path: 'Agent/Category/:choice', title: 'SmartShield | Agent | Categories', component: CategoryComponent },
  { path: 'Agent/View/:choice', title: 'SmartShield | Agent | View', component: AgentViewComponent },
  //Client
  { path: 'Client/Policies', title: 'SmartShield | Client | Client Policies', component: CPoliciesComponent },
  { path: 'Client/Nominee', title: 'SmartShield | Client | Nominees', component: NomineeComponent },
  { path: 'Client/View/:choice', title: 'SmartShield | Client | View', component: ClientViewComponent },
  { path: 'Client/ViewDetails/:policyId', title: 'SmartShield | Client | ViewDetails', component: DetailsComponent },
  { path: 'Client/Payment/:clientpolicyId', title: 'SmartShield | Client | Payment', component: PaymentComponent },
  { path: 'Client/Penalty/:clientpolicyId', title: 'SmartShield | Client | Penalty Payment', component: PenaltyComponent },
  //Common
  { path: 'Reports',title : 'SmartShield | Report', component: ReportComponent },
  { path: 'Profile/:userid',title : 'SmartShield | Profile', component: ProfileComponent },
  { path: 'Register',title : 'SmartShield | Register', component: RegisterComponent },
  { path: 'Login',title : 'SmartShield | Login', component: LoginComponent },
  { path: 'Denial',title : 'SmartShield | Access Denied', component: DenialComponent },
  { path: '', title: 'SmartShield | Home', redirectTo: 'Home', pathMatch: 'full' },
  { path: '**', title: 'SmartShield | 404', component: ErrorComponent }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
