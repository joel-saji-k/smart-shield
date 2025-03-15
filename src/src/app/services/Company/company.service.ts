import { query } from '@angular/animations';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { error } from 'jquery';
import { Observable } from 'rxjs';
import { Agent } from 'src/app/models/agent';
import { Agentcompany } from 'src/app/models/agentcompany';
import { Company } from 'src/app/models/company';
import { Policy } from 'src/app/models/policy';
import { Policyterm } from 'src/app/models/policyterm';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  policyterm: Policyterm;
  constructor(private http: HttpClient,private route : Router) { }

  GetCompany(userId: number): Observable<Company> {
    let p = new HttpParams().append('userID', userId);
    return this.http.get<Company>(environment.baseApiUrl + '/api/Company/GetCompany', { params: p });
  }

  AddPolicy(policy: Policy) {
    const formData = new FormData();
    for (const prop in policy) {
      if (policy.hasOwnProperty(prop)) {
        formData.append(prop, policy[prop]);
      }
    }
    this.http.post(environment.baseApiUrl + '/api/Company/AddPolicy',formData).subscribe((res : Policy) => {
      console.log(res);
      alert("Policy Added Sucessfully\nPlease Add Policy Terms on Next page");
    }
  );
}

GetPolicy(policyId : number) : Observable<Policy>
{
  let queries = new HttpParams().append('policyId', policyId);
  let dbpolicy: Observable<Policy> = this.http.get<Policy>(environment.baseApiUrl + '/api/Company/GetPolicy', { params: queries });
  return dbpolicy;
}

AddPolicyTerm(policyterm : Policyterm){
  const formData = new FormData();
  for (const prop in policyterm) {
    if (policyterm.hasOwnProperty(prop)) {
      formData.append(prop, policyterm[prop]);
    }
  }
  this.http.post(environment.baseApiUrl + '/api/Company/AddPolicyTerm', formData).subscribe(res => {
    alert("Policy Term Added Successfully");
  });
}

GetAllCompany(): Observable < Company[] > {
  return this.http.get<Company[]>(environment.baseApiUrl + '/api/Company/GetAllCompany');
}

Apply(companyId : number, agentId : number)
{

  const formData = new FormData();
  formData.append('companyId', companyId.toString());
  formData.append('agentId', agentId.toString());
  this.http.post(environment.baseApiUrl + '/api/Agent/ApplyCompany', formData).subscribe(res => {
    alert("Applied for Company Authorization!");
  },
    error => {
      alert("Already Applied!!");
    });
}

GetAgents(companyId : number): Observable < Agentcompany[] > {
  let queries = new HttpParams().append('companyId', companyId);
  return this.http.get<Agentcompany[]>(environment.baseApiUrl + '/api/Company/ViewAgents', { params: queries });
}

AgentName(agentId : number) : string
{
  let queries = new HttpParams().append('agentId', agentId);
  let Name: string;
  this.http.get<Agent>(environment.baseApiUrl + '/api/Agent/GetAgentById', { params: queries }).subscribe(res => {
    Name = res.agentName;
  });
  return Name;
}

ChangeStatus(status : number, id : number)
{
  const formData = new FormData();
  formData.append('id', id.toString());
  formData.append('status', status.toString());
  this.http.post<Agentcompany>(environment.baseApiUrl + '/api/Company/ChangeAgentCompanyStatus', formData).subscribe((res) => {
    alert('Status Updated!');
  }
  );
}

ChangePolicyStatus(status : number,policyId : number)
{
  const formData = new FormData();
  formData.set('policyId',policyId.toString());
  formData.set('status',status.toString());
  this.http.put(environment.baseApiUrl+'/api/Company/ChangePolicyStatus',formData).subscribe((res : Policy)=>{
    alert('Blocked Policy');
    this.route.navigate[("/Home/Company")];
  },
  error=>{
    alert("Action Failed!!");
  })
}
}
