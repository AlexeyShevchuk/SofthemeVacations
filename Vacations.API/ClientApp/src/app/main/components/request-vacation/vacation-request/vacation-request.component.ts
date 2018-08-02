import { Component, OnInit, OnChanges, DoCheck } from '@angular/core';
import { Location, getLocaleFirstDayOfWeek } from '@angular/common';
import { ToastrService } from 'ngx-toastr';

import { EditService } from '../../../services/edit.service';
import { VacationService } from '../../../services/vacation.service';

import { Employee } from '../../edit-profile/models/employee.model';
import { VacModel } from './vacation-request.model';
import { VacType } from './vacation-types.model';

@Component({
  selector: 'app-vacation-request',
  templateUrl: './vacation-request.component.html',
  styleUrls: ['./vacation-request.component.scss']
})

export class VacationRequestComponent implements OnInit {


  employee: Employee = <Employee>{};
  vacation: VacModel = <VacModel>{};
  vacTypes: VacType[] = [];
 

  constructor(private location: Location, private service: VacationService, private emplService: EditService, private toast: ToastrService) { }

  cancel() {
    this.location.back();
  }

  dateDiff: any = 'XX';

  ngOnInit() {
    const successfnEmployee = (response) => {
      this.employee = response;
      console.log(this.employee);
    };

    const successfnVacationTypes = (response) => {
      this.vacTypes = response;
      console.log(response);
      console.log(this.vacTypes);
    };

    const errorfn = () => { };
    const completefn = () => { };

    this.service.getVacationType().subscribe(successfnVacationTypes, errorfn, completefn);
    this.emplService.getEmployee().subscribe(successfnEmployee, errorfn, completefn);  
  }

  calculateDate() {
    console.log("was trying to parse");
    this.DaysInVac(
      this.parseDate(this.vacation.StartVocationDate),
      this.parseDate(this.vacation.EndVocationDate));
  }
  
  parseDate(dateString: any): Date {
    if (dateString) {
      return new Date(dateString);
    } else {
      return null;
    }
  }

  DaysInVac(frst, lst) {
  
    this.dateDiff = (lst - frst) / 1000 / 60 / 60 / 24;
  }

  Send() {
    
    console.log(this.employee);
    console.log(this.vacation.VacationTypesId);
    this.vacation.EmployeeId =this.employee.EmployeeId;
    this.service.SendVacation(this.vacation).subscribe(response => {
      this.toast.success("You successfully send vacation request", "");
      this.location.back();
    });
  }
}
