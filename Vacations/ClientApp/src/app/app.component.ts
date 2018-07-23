import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
  constructor(private toastr: ToastrService) {}

  showSuccess() {
    this.toastr.success('Hello world!', 'Toastr fun!');
  }

  showError() {
    this.toastr.error('Error message', 'Toastr error!');
  }
  
  showInfo() {
    this.toastr.info('Info message', 'Toastr info!');
  }
  
  showWarning() {
    this.toastr.warning('Warnig message', 'Toastr warning!');
  }
}
