import { Component, OnInit } from '@angular/core';
import { ImageService } from '../../services/image.service';

@Component({
  selector: 'app-top-navigation',
  templateUrl: './top-navigation.component.html',
  styleUrls: ['./top-navigation.component.scss']
})
export class TopNavigationComponent implements OnInit {

  imgUrl: string;

  constructor(private imgService: ImageService) { }

  ngOnInit() {
    this.imgService.getImgUrl().subscribe(
      response => {this.imgUrl = response; console.log(response); console.log(this.imgUrl);},
      () => this.imgUrl = "default");
  }
}
