import {Component, OnInit} from '@angular/core';
import {Observable, Subject} from "rxjs";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  observableData1 = '';
  observableData2 = '';
  subjectData1 = '';
  subjectData2 = '';

  getObservable() {
    let observable = new Observable<any>(e=>{
      const a :number = Math.floor(Math.random() * 99) +1;
      console.log(a);
      setTimeout(()=>{e.next(a)}, 2000);
      const b :number = Math.floor(Math.random() * 99) +1;
      console.log(b);
      setTimeout(()=>{e.next(b)}, 4000);
      const c :number = Math.floor(Math.random() * 99) +1;
      console.log(c);
      setTimeout(()=>{e.next(c)}, 6000);
    });

    observable.subscribe(e => {
      this.observableData1 = e;
      console.log("s1-------")
      console.log(e);
      console.log("e1-------")
    })

    // observable.subscribe(e => {
    //   this.observableData2 = e;
    //   console.log("s2-------")
    //   console.log(e);
    //   console.log("e2-------")
    // })
  }

  getSubject() {
    let subject = new Subject<any>();

    subject.subscribe(e =>{
      this.subjectData1 = e;
      console.log("s1-------")
      console.log(e);
      console.log("e1-------")
    })

    subject.subscribe(e =>{
      this.subjectData2 = e;
      console.log("s2-------")
      console.log(e);
      console.log("e2-------")
    })

    const a :number = Math.floor(Math.random() * 99) +1;
    console.log("a");
    // subject.next(a);
    setTimeout(()=>{subject.next(a)}, 2000);
    const b :number = Math.floor(Math.random() * 99) +1;
    console.log(b);
    // subject.next(b);
    setTimeout(()=>{subject.next(b)}, 4000);
    const c :number = Math.floor(Math.random() * 99) +1;
    console.log(c);
    // subject.next(c);
    setTimeout(()=>{subject.next(c)}, 6000);
  }

  ngOnInit(): void {
  }
}
