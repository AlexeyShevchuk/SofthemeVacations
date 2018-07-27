import { Component, OnInit } from '@angular/core';
import { VacationService } from '../../services/vacation.service';
import { Vacation } from '../profile/my-vacations/vacation.model';

@Component({
  selector: 'app-list-of-vacation-requests',
  templateUrl: './list-of-vacation-requests.component.html',
  styleUrls: ['./list-of-vacation-requests.component.scss']
})
export class ListOfVacationRequestsComponent implements OnInit {

  vacationsList: Vacation[];

  constructor(private service:VacationService) { }

  ngOnInit() {
    this.service.getVacations()
    .subscribe(response => this.vacationsList = response);
  }

}
