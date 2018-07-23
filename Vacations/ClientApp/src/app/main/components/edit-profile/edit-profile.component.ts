import { Component, OnInit } from '@angular/core';

import { EditService } from '../../services/edit.service';
import { Employee } from '../profile/my-profile/employee.model';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.scss']
})
export class EditProfileComponent implements OnInit {
  title = 'profile';
  employee: Employee;
  
  constructor(private service: EditService) { }

  ngOnInit() { }
  //save(): void {
  //  this.service.updateEmployee(this.employee)
  //    .subscribe(() => this.location.back();;
  //}

}
