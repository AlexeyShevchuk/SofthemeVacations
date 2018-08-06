import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { Component, OnInit } from '@angular/core';
import { MatChipInputEvent } from '@angular/material';

import { ProfileService } from '../../services/profile.service';

import { Profile } from '../profile/my-profile/profile.model';
import { ActivatedRoute } from '@angular/router';
import { TeamService } from '../../services/team.service';
import { Team } from '../edit-profile/models/team.model';
import { User } from './user.model';

@Component({
  selector: 'app-edit-team-profile',
  templateUrl: './edit-team-profile.component.html',
  styleUrls: ['./edit-team-profile.component.scss']
})

export class EditTeamProfileComponent implements OnInit {
  id: string;
  visible = true;
  selectable = true;
  removable = true;
  addOnBlur = true;
  readonly separatorKeysCodes: number[] = [ENTER, COMMA];
  team: Team = <Team>{};
  teamLead: string;
  
  users: User[] = [];

  employees: Profile[] = [];
  noTeamEmpl: Profile[] = [];

  constructor(private service: ProfileService, 
    private teamService: TeamService,
    private activateRoute: ActivatedRoute) {
      this.id = this.activateRoute.snapshot.paramMap.get('id');
    }

  ngOnInit() {
    this.service.getEmployees()
      .subscribe(response => {
        this.employees = response;
        for (let item of this.employees) {
          let i = 0;
          if (!item.TeamId) {
            this.noTeamEmpl[i] = item;
            this.users[i].name = this.noTeamEmpl[i].Name;
            this.users[i].surname = this.noTeamEmpl[i].Surname;
            console.log(this.users);
            i=i+1;            
          }
        } 
    });

    this.teamService.getTeam(this.id).subscribe(response => {
      this.team = response;
      console.log(response);
      console.log(this.team);
      this.teamLead = this.team.TeamLeadName + ' ' +this.team.TeamLeadSurname;
    });
    
  }

  add(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value;

    // Add our user
    if ((value || '').trim()) {
      console.log("Add user");
      this.users.push({ name: value.trim(),  surname: value.trim()  });
    }

    // Reset the input value
    if (input) {
      input.value = '';
    }
  }

  remove(user: User): void {
    const index = this.users.indexOf(user);

    if (index >= 0) {
      this.users.splice(index, 1);
    }
  }

  OnClick(empl: Profile){
    let employee = empl.Name + ' ' + empl.Surname;
    console.log("Click on user");
    this.users.push({ name: employee.trim(), surname: employee.trim() });
  }
}
