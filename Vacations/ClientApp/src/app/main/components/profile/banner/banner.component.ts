import { Component, OnInit } from '@angular/core';

import { Employee } from '../my-profile/employee.model';
import { EmployeeService } from '../../../services/employee.service';

@Component({
  selector: 'app-banner',
  templateUrl: './banner.component.html',
  styleUrls: ['./banner.component.scss']
})
export class BannerComponent implements OnInit {

  employee: Employee;
  
  constructor(private service: EmployeeService) { }

  ngOnInit() {
    this.service.getEmployee().subscribe(response => this.employee = response);
  }

}
