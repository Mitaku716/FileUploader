import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as XLSX from 'xlsx';
import 'lodash';

declare var _:any;

@Component({
  selector: 'app-upload-system',
  templateUrl: './upload-system.component.html'
})

export class UploadSystemComponent {
  file: File;
  arrayBuffer: any;
  headers: any[];
  fieldNames: any;
  oldValue: any;

  addfile(event)     
  {  
    this.file= event.target.files[0];     
    let fileReader = new FileReader();    
    fileReader.readAsArrayBuffer(this.file);     
    fileReader.onload = (e) => {    
      this.arrayBuffer = fileReader.result;    
      var data = new Uint8Array(this.arrayBuffer);    
      var arr = new Array();    
      for(var i = 0; i != data.length; ++i) arr[i] = String.fromCharCode(data[i]);    
      var bstr = arr.join("");    
      var workbook = XLSX.read(bstr, {type:"binary"});    
      var first_sheet_name = workbook.SheetNames[0];    
      var worksheet = workbook.Sheets[first_sheet_name];    
      var arraylist = XLSX.utils.sheet_to_json(worksheet,{raw:true});     
      this.ProcessHeaders(arraylist);

    }
  }

   EnsureNonDuplicateFieldNames(fieldName, index){
     if(this.headers[index].duplicated){
       
     }

     //if headers already has a fieldName in use
     var searchIndex = _.findIndex(this.headers, function(o){ return o.fieldName == fieldName; })
     //if it doesnt then set the fieldName to the newly chosen field
     if(searchIndex == -1){
      this.headers[index].duplicated = true;
      this.headers[searchIndex].duplicated = true;
     }
     this.headers[index].fieldName = fieldName;

   }

  ProcessHeaders(elements){
    if(elements == null || elements.length == 0){
      return;
    }
    var headerList = Object.keys(elements[0]);
    this.headers = [];
    for(var i = 0; i < headerList.length; i++){
      this.headers.push({
        key: headerList[i],
        fieldName: null,
        duplicated: false
      })
    }
    debugger;  

  }

  // DuplicateEntry(){

  // }
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<string[]>(baseUrl + 'FileUploader').subscribe(result => {
      this.fieldNames = result.map(x=> { return ({id: x, value: false})});
    }, error => console.error(error));
  }
}

interface Document {
  date: string;
  portfolio: string;
  data: any;
}
