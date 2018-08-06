import { Component, OnInit } from '@angular/core';
import { ImageService } from '../../services/image.service';
import { Routes, Router } from '@angular/router';
import { EditService } from '../../services/edit.service';
import { Employee } from '../edit-profile/models/employee.model';

@Component({
  selector: 'app-top-navigation',
  templateUrl: './top-navigation.component.html',
  styleUrls: ['./top-navigation.component.scss']
})
export class TopNavigationComponent implements OnInit {

  employee: Employee = <Employee>{};
  imgUrl: string;

  constructor(private service: EditService, private router: Router) { }

  ngOnInit() {
    this.imgUrl = '../../../../assets/user-profile-icon.svg'
    let employee: Employee;
    this.service.getEmployee().subscribe(response => {
      this.employee = response;
      this.imgUrl = this.employee.ImgUrl
      console.log(this.employee);
      console.log(response);
    });
  }
  viewdiv(id) {
    const el = document.getElementById(id);
    if (el.style.display === 'block') {
      el.style.display = 'none';

    } else {
      el.style.display = 'block';
    }
  }
  logout()
  {
    localStorage.setItem("token", "");
    localStorage.setItem("role", "");
    this.router.navigate(["/auth"])
  }
}
