import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';

import { Profile } from './profile.model';
import { ProfileService } from '../../../services/profile.service';
import { ImageService } from '../../../services/image.service';

@Component({
  selector: 'app-my-profile',
  templateUrl: './my-profile.component.html',
  styleUrls: ['./my-profile.component.scss']
})

export class MyProfileComponent implements OnInit {
  title = 'profile';
  employee: Profile = <Profile>{};
  constructor(private service: ProfileService, private router: Router, private imgService: ImageService) { }
  
  toEdit(empl: Profile) {
    this.router.navigate(['/edit-profile']);
  }

  imgUrl: string;

  ngOnInit() {
    this.imgUrl = '../../../../assets/user-profile-icon.svg';
    this.service.getProfile().subscribe(response => {
      this.employee = response;
      this.imgUrl = this.employee.ImgUrl
      console.log(this.employee);
      console.log(response);
    });
  }
}
