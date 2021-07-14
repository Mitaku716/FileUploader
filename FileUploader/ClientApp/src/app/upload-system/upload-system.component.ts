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
  httpClient: HttpClient;

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
      var client = this;
      if(this.headers[index].duplicated){
       this.headers[index].duplicated = false;
       if(fieldName == "Select a Field"){
         return;
       }
       var duplicates = _.filter(this.headers, function(o){ return (o.fieldName == client.headers[index].fieldName && o.duplicated == true); });
       if(duplicates.length == 1){
        this.headers[_.findIndex(this.headers, function(o){return (o.fieldName == client.headers[index].fieldName && o.duplicated == true)})].duplicated = false;
       }
     }
     this.headers[index].fieldName = fieldName;
     if(fieldName == "Select a Field"){
      return;
    }
     var itemsWithFieldName = _.filter(client.headers, function(o){return (o.fieldName == client.headers[index].fieldName)});
     if(itemsWithFieldName.length > 1){
       for(var i = 0; i < itemsWithFieldName.length; i++){
         client.headers[_.findIndex(client.headers, itemsWithFieldName[i])].duplicated = true;
       }
     }
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
  }

  SubmitToController(){

  
  }
  
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<FieldName[]>(baseUrl + 'FileUploader').subscribe(result => {
      this.fieldNames = result.map(x=> { return ({id: x.name, propertyType: x.propertyType, value: false})});
    }, error => console.error(error));
  }
}

interface FieldName {
  propertyType: string;
  name: string;
}
