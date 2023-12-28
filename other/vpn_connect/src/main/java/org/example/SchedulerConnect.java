//package org.example;
//
//import org.quartz.*;
//import org.quartz.impl.StdSchedulerFactory;
//
//import java.util.Date;
//
//import static org.quartz.CronScheduleBuilder.cronSchedule;
//import static org.quartz.JobBuilder.newJob;
//import static org.quartz.TriggerBuilder.newTrigger;
//
//public class SchedulerConnect {
//    public static void run(String cron) {
//        try {
//            SchedulerFactory sf = new StdSchedulerFactory();
//            Scheduler sched = sf.getScheduler();
//
//            JobDetail job = newJob(JobTestRequest.class)
//                    .withIdentity("myJob", "group1")
//                    .build();
//
//            Date startTime = DateBuilder.nextGivenMinuteDate(new Date(), 5);
//
//            Trigger trigger = newTrigger()
//                    .withIdentity("myTrigger", "group1")
//                    .startAt(startTime)
//                    .withSchedule(cronSchedule(cron))
//                    .build();
//
//            sched.scheduleJob(job, trigger);
//            sched.start();
//        } catch (SchedulerException e) {
//            throw new RuntimeException(e);
//        }
//    }
//}
