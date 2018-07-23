import { Component, OnInit } from '@angular/core';

import { EmployeeService } from '../../../services/employee.service';
import { Employee } from './employee.model';

@Component({
  selector: 'app-my-profile',
  templateUrl: './my-profile.component.html',
  styleUrls: ['./my-profile.component.scss']
})

export class MyProfileComponent implements OnInit {
  title = 'profile';
  employee: Employee;
  
  constructor(private service: EmployeeService) { }

  ngOnInit() {
    this.service.getEmployee().subscribe(response => this.employee = response);
  }


}
