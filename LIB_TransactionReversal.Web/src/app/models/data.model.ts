


export interface Login {
  id: number;
  branch: string;
 fullName: string;
  userName: string;
  password: String;
  role: String;
  branchCode:string
  updatedBy:string;
  updatedDate:String;
  status:string,
 

}
export interface TokenResponse {
  token: string;
userData:{
  id: number;
  branch: string;
 fullName: string;
  userName: string;
  password: String;
  role: string;
  branchCode:string
  updatedBy:string;
  updatedDate:String;
  status:string
}  

}

export interface UserData {
  branch:number;
   branchName:number;
   userName:number ;
  fullName:string;
  role :string;
  createdDate:string; 
  updatedDate :string;
status:string

}

export interface Receipt {
  updatedDate: string,
  updatedBy: string,
  status: string,
  id: number,
  inputing_Branch: string,
  transaction_Date:string,
  account_No: string,
  amount1: number,
   phone_No :string,
   address :string,
   tinNo :string,
   debitor_Name :string,
   paymentMethod :string,
  refno: string,
  branch: string,
  cAccountNo: string,
  createdBy: string,
  approvedBy: string,
  messsageNo: string,
  paymentNo: string,
  paymentType: string
  serviceFee: number,    
  serviceFee2: number,
  vat: number,
  vat2: number,
}
