import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { environment } from '../../../environments/environment';
import { Team } from '../components/edit-profile/models/team.model';

@Injectable()
export class TeamService {
    constructor (private http: HttpClient) { }

    ContentTypeHeader = new HttpHeaders ({
        'Content-Type':  'application/json'});
        
    getTeams(): Observable<Team[]> {
        let requestUrl = environment.baseUrl + '/teams';
        return this.http.get<Team[]>(`${requestUrl}`);
    }

    addTeam(team: Team): Observable<Team> {
        console.log("Service add team works");
        let requestUrl = environment.baseUrl + '/teams';
        const data = JSON.stringify(team);

        return this.http.post<Team>(requestUrl, data, { headers: this.ContentTypeHeader }).map(() => team);
    }

    getTeam(id: string): Observable<Team> {
        let idUrl = id;
        console.log(id);
        console.log(idUrl);
        let currentUrl = '/teams/' + idUrl;
        let requestUrl = environment.baseUrl + currentUrl;
        return this.http.get<Team>(`${requestUrl}`);
    }
}