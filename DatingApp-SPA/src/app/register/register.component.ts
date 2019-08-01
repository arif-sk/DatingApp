import { Component, OnInit, Input } from '@angular/core';
import { HttpClient } from 'selenium-webdriver/http';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};
  @Input() valuesFromHome: any;
  constructor() { }

  ngOnInit() {
  }
  register(model: any) { 
    console.log(this.model);
  }
  cancel() {
    console.log('cancel');
  }
}
